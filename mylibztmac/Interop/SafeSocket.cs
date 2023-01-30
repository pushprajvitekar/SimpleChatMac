//using System.Net.Cache;
//using System.Net.Sockets;
//using System.Net.NetworkInformation;
//using System.Net.WebSockets;
//using System.Security;
//using System.Runtime.InteropServices;
//using System.Runtime.CompilerServices;
//using System.Threading;
//using System.Security.Authentication.ExtendedProtection;
//using System.Security.Permissions;
//using System.ComponentModel;
//using System.Text;
//using System.Globalization;
//using Microsoft.Win32.SafeHandles;
//using System.Runtime.ConstrainedExecution;
//using System.Diagnostics.CodeAnalysis;
//using System.Collections.Generic;
//using System;

//namespace mylibzt.Interop
//{

//    //
//    // This is a helper class for debugging GC-ed handles that we define.
//    // As a general rule normal code path should always destroy handles explicitly
//    //
////    internal abstract class DebugSafeHandleMinusOneIsInvalid : SafeHandleMinusOneIsInvalid
////    {
////        string m_Trace;

////        protected DebugSafeHandleMinusOneIsInvalid(bool ownsHandle) : base(ownsHandle)
////        {
////            Trace();
////        }

////        [EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
////        private void Trace()
////        {
////            m_Trace = "WARNING! GC-ed  >>" + this.GetType().FullName + "<< (should be excplicitly closed) \r\n";
////            GlobalLog.Print("Creating SafeHandle, type = " + this.GetType().FullName);
////#if TRAVE
////            (new FileIOPermission(PermissionState.Unrestricted)).Assert();
////            string stacktrace = Environment.StackTrace;
////            m_Trace += stacktrace;
////            FileIOPermission.RevertAssert();
////#endif //TRAVE
////        }

////        ~DebugSafeHandleMinusOneIsInvalid()
////        {
////            GlobalLog.SetThreadSource(ThreadKinds.Finalization);
////            GlobalLog.Print(m_Trace);
////        }
////    }



//    ///////////////////////////////////////////////////////////////
//    //
//    // This class implements a safe socket handle.
//    // It uses an inner and outer SafeHandle to do so.  The inner
//    // SafeHandle holds the actual socket, but only ever has one
//    // reference to it.  The outer SafeHandle guards the inner
//    // SafeHandle with real ref counting.  When the outer SafeHandle
//    // is cleaned up, it releases the inner SafeHandle - since
//    // its ref is the only ref to the inner SafeHandle, it deterministically
//    // gets closed at that point - no ----s with concurrent IO calls.
//    // This allows Close() on the outer SafeHandle to deterministically
//    // close the inner SafeHandle, in turn allowing the inner SafeHandle
//    // to block the user thread in case a g----ful close has been
//    // requested.  (It's not legal to block any other thread - such closes
//    // are always abortive.)
//    //
//    ///////////////////////////////////////////////////////////////
//    ///

//    /*
//    [SuppressUnmanagedCodeSecurity]
////#if DEBUG
////    internal class SafeCloseSocket : DebugSafeHandleMinusOneIsInvalid
////#else
//    internal class SafeCloseSocket : SafeHandleMinusOneIsInvalid
////#endif
//    {
//        protected SafeCloseSocket() : base(true) { }

//        private InnerSafeCloseSocket m_InnerSocket;
//        private volatile bool m_Released;
//#if DEBUG
//        private InnerSafeCloseSocket m_InnerSocketCopy;
//#endif

//        public override bool IsInvalid
//        {
//            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
//            get
//            {
//                return IsClosed || base.IsInvalid;
//            }
//        }

//        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
//        private void SetInnerSocket(InnerSafeCloseSocket socket)
//        {
//            m_InnerSocket = socket;
//            SetHandle(socket.DangerousGetHandle());
//#if DEBUG
//            m_InnerSocketCopy = socket;
//#endif
//        }

//        private static SafeCloseSocket CreateSocket(InnerSafeCloseSocket socket)
//        {
//            SafeCloseSocket ret = new SafeCloseSocket();
//            CreateSocket(socket, ret);
//            return ret;
//        }

//        protected static void CreateSocket(InnerSafeCloseSocket socket, SafeCloseSocket target)
//        {
//            if (socket != null && socket.IsInvalid)
//            {
//                target.SetHandleAsInvalid();
//                return;
//            }

//            bool b = false;
//            RuntimeHelpers.PrepareConstrainedRegions();
//            try
//            {
//                socket.DangerousAddRef(ref b);
//            }
//            catch
//            {
//                if (b)
//                {
//                    socket.DangerousRelease();
//                    b = false;
//                }
//            }
//            finally
//            {
//                if (b)
//                {
//                    target.SetInnerSocket(socket);
//                    socket.Close();
//                }
//                else
//                {
//                    target.SetHandleAsInvalid();
//                }
//            }
//        }

//        //internal unsafe static SafeCloseSocket CreateWSASocket(byte* pinnedBuffer)
//        //{
//        //    return CreateSocket(InnerSafeCloseSocket.CreateWSASocket(pinnedBuffer));
//        //}

//        internal static SafeCloseSocket CreateWSASocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
//        {
//            return CreateSocket(InnerSafeCloseSocket.CreateWSASocket(addressFamily, socketType, protocolType));
//        }

//        internal static SafeCloseSocket Accept(
//                                            SafeCloseSocket socketHandle,
//                                            byte[] socketAddress,
//                                            ref int socketAddressSize
//                                            )
//        {
//            return CreateSocket(InnerSafeCloseSocket.Accept(socketHandle, socketAddress, ref socketAddressSize));
//        }

//        protected override bool ReleaseHandle()
//        {
//            m_Released = true;
//            InnerSafeCloseSocket innerSocket = m_InnerSocket == null ? null : Interlocked.Exchange<InnerSafeCloseSocket>(ref m_InnerSocket, null);
//            if (innerSocket != null)
//            {
//                innerSocket.DangerousRelease();
//            }
//            return true;
//        }

//        internal void CloseAsIs()
//        {
//            RuntimeHelpers.PrepareConstrainedRegions();
//            try { }
//            finally
//            {
//#if DEBUG
//                // If this throws it could be very bad.
//                try
//                {
//#endif
//                    InnerSafeCloseSocket innerSocket = m_InnerSocket == null ? null : Interlocked.Exchange<InnerSafeCloseSocket>(ref m_InnerSocket, null);
//                    Close();
//                    if (innerSocket != null)
//                    {
//                        // Wait until it's safe.
//                        while (!m_Released)
//                        {
//                            Thread.SpinWait(1);
//                        }

//                        // Now free it with blocking.
//                        innerSocket.BlockingRelease();
//                    }
//#if DEBUG
//                }
//                catch (Exception /*exception*/)
//                {
//                    //if (!NclUtilities.IsFatal(exception))
//                    //{
//                    //    GlobalLog.Assert("SafeCloseSocket::CloseAsIs(handle:" + handle.ToString("x") + ")", exception.Message);
//                    //}
//                    throw;
//                }
//#endif
//            }
//        }

//        internal class InnerSafeCloseSocket : SafeHandleMinusOneIsInvalid
//        {
//            protected InnerSafeCloseSocket() : base(true) { }

//            private static readonly byte[] tempBuffer = new byte[1];
//            private bool m_Blockable;

//            public override bool IsInvalid
//            {
//                [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
//                get
//                {
//                    return IsClosed || base.IsInvalid;
//                }
//            }

//            // This method is implicitly reliable and called from a CER.
//            protected override bool ReleaseHandle()
//            {
//                bool ret = false;

//#if DEBUG
//                try
//                {
//#endif
//                   // GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ")");

//                    SocketError errorCode;

//                    // If m_Blockable was set in BlockingRelease, it's safe to block here, which means
//                    // we can honor the linger options set on the socket.  It also means closesocket() might return WSAEWOULDBLOCK, in which
//                    // case we need to do some recovery.
//                    if (m_Blockable)
//                    {
//                       // GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") Following 'blockable' branch.");

//                        errorCode = UnsafeNclNativeMethods.SafeNetHandles.closesocket(handle);
//#if DEBUG
//                        m_CloseSocketHandle = handle;
//                        m_CloseSocketResult = errorCode;
//#endif
//                        if (errorCode == SocketError.SocketError) errorCode = (SocketError)Marshal.GetLastWin32Error();
//                        //GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") closesocket()#1:" + errorCode.ToString());

//                        // If it's not WSAEWOULDBLOCK, there's no more recourse - we either succeeded or failed.
//                        if (errorCode != SocketError.WouldBlock)
//                        {
//                            return ret = errorCode == SocketError.Success;
//                        }

//                        // The socket must be non-blocking with a linger timeout set.
//                        // We have to set the socket to blocking.
//                        int nonBlockCmd = 0;
//                        errorCode = UnsafeNclNativeMethods.SafeNetHandles.ioctlsocket(
//                            handle,
//                            IoctlSocketConstants.FIONBIO,
//                            ref nonBlockCmd);
//                        if (errorCode == SocketError.SocketError) errorCode = (SocketError)Marshal.GetLastWin32Error();
//                        //GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") ioctlsocket()#1:" + errorCode.ToString());

//                        // This can fail if there's a pending WSAEventSelect.  Try canceling it.
//                        if (errorCode == SocketError.InvalidArgument)
//                        {
//                            errorCode = UnsafeNclNativeMethods.SafeNetHandles.WSAEventSelect(
//                                handle,
//                                IntPtr.Zero,
//                                AsyncEventBits.FdNone);
//                            //GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") WSAEventSelect():" + (errorCode == SocketError.SocketError ? (SocketError)Marshal.GetLastWin32Error() : errorCode).ToString());

//                            // Now retry the ioctl.
//                            errorCode = UnsafeNclNativeMethods.SafeNetHandles.ioctlsocket(
//                                handle,
//                                IoctlSocketConstants.FIONBIO,
//                                ref nonBlockCmd);
//                            //GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") ioctlsocket#2():" + (errorCode == SocketError.SocketError ? (SocketError)Marshal.GetLastWin32Error() : errorCode).ToString());
//                        }

//                        // If that succeeded, try again.
//                        if (errorCode == SocketError.Success)
//                        {
//                            errorCode = UnsafeNclNativeMethods.SafeNetHandles.closesocket(handle);
//#if DEBUG
//                            m_CloseSocketHandle = handle;
//                            m_CloseSocketResult = errorCode;
//#endif
//                            if (errorCode == SocketError.SocketError) errorCode = (SocketError)Marshal.GetLastWin32Error();
//                           // GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") closesocket#2():" + errorCode.ToString());

//                            // If it's not WSAEWOULDBLOCK, there's no more recourse - we either succeeded or failed.
//                            if (errorCode != SocketError.WouldBlock)
//                            {
//                                return ret = errorCode == SocketError.Success;
//                            }
//                        }

//                        // It failed.  Fall through to the regular abortive close.
//                    }

//                    // By default or if CloseAsIs() path failed, set linger timeout to zero to get an abortive close (RST).
//                    Linger lingerStruct;
//                    lingerStruct.OnOff = 1;
//                    lingerStruct.Time = 0;

//                    errorCode = UnsafeNclNativeMethods.SafeNetHandles.setsockopt(
//                        handle,
//                        SocketOptionLevel.Socket,
//                        SocketOptionName.Linger,
//                        ref lingerStruct,
//                        4);
//#if DEBUG
//                    m_CloseSocketLinger = errorCode;
//#endif
//                    if (errorCode == SocketError.SocketError) errorCode = (SocketError)Marshal.GetLastWin32Error();
//                   // GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") setsockopt():" + errorCode.ToString());

//                    if (errorCode != SocketError.Success && errorCode != SocketError.InvalidArgument && errorCode != SocketError.ProtocolOption)
//                    {
//                        // Too dangerous to try closesocket() - it might block!
//                        return ret = false;
//                    }

//                    errorCode = UnsafeNclNativeMethods.SafeNetHandles.closesocket(handle);
//#if DEBUG
//                    m_CloseSocketHandle = handle;
//                    m_CloseSocketResult = errorCode;
//#endif
//                   // GlobalLog.Print("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ") closesocket#3():" + (errorCode == SocketError.SocketError ? (SocketError)Marshal.GetLastWin32Error() : errorCode).ToString());

//                    return ret = errorCode == SocketError.Success;
//#if DEBUG
//                }
//                catch (Exception exception)
//                {
//                    //if (!NclUtilities.IsFatal(exception))
//                    //{
//                    //    GlobalLog.Assert("SafeCloseSocket::ReleaseHandle(handle:" + handle.ToString("x") + ")", exception.Message);
//                    //}
//                    ret = true;  // Avoid a second assert.
//                    throw;
//                }
//                finally
//                {
//                    m_CloseSocketThread = Thread.CurrentThread.ManagedThreadId;
//                    m_CloseSocketTick = Environment.TickCount;
//                   // GlobalLog.Assert(ret, "SafeCloseSocket::ReleaseHandle(handle:{0:x})|ReleaseHandle failed.", handle);
//                }
//#endif
//            }

//#if DEBUG
//            private IntPtr m_CloseSocketHandle;
//            private SocketError m_CloseSocketResult = unchecked((SocketError)0xdeadbeef);
//            private SocketError m_CloseSocketLinger = unchecked((SocketError)0xdeadbeef);
//            private int m_CloseSocketThread;
//            private int m_CloseSocketTick;
//#endif

//            // Use this method to close the socket handle using the linger options specified on the socket.
//            // Guaranteed to only be called once, under a CER, and not if regular DangerousRelease is called.
//            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
//            internal void BlockingRelease()
//            {
//                m_Blockable = true;
//                DangerousRelease();
//            }

//            internal unsafe static InnerSafeCloseSocket CreateWSASocket(byte* pinnedBuffer)
//            {
//                //-1 is the value for FROM_PROTOCOL_INFO
//                InnerSafeCloseSocket result = UnsafeNclNativeMethods.OSSOCK.WSASocket((AddressFamily)(-1), (SocketType)(-1), (ProtocolType)(-1), pinnedBuffer, 0, SocketConstructorFlags.WSA_FLAG_OVERLAPPED);
//                if (result.IsInvalid)
//                {
//                    result.SetHandleAsInvalid();
//                }
//                return result;
//            }

//            internal static InnerSafeCloseSocket CreateWSASocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
//            {
//                InnerSafeCloseSocket result = UnsafeNclNativeMethods.OSSOCK.WSASocket(addressFamily, socketType, protocolType, IntPtr.Zero, 0, SocketConstructorFlags.WSA_FLAG_OVERLAPPED);
//                if (result.IsInvalid)
//                {
//                    result.SetHandleAsInvalid();
//                }
//                return result;
//            }

//            internal static InnerSafeCloseSocket Accept(SafeCloseSocket socketHandle, byte[] socketAddress, ref int socketAddressSize)
//            {
//                InnerSafeCloseSocket result = UnsafeNclNativeMethods.SafeNetHandles.accept(socketHandle.DangerousGetHandle(), socketAddress, ref socketAddressSize);
//                if (result.IsInvalid)
//                {
//                    result.SetHandleAsInvalid();
//                }
//                return result;
//            }
//        }
//    }
//}
//*/
