using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace mylibzt.Interop
{
 //   public class ZTSocketWrapper
 //   {
 //       public ZTSocketWrapper()
 //       {

 //           using (var socket = LibZtSocketHandle.Create())
 //           {
 //               // use the socket
 //           }
 //       }
 //   }


 //internal class LibZtSocketHandle : SafeHandleMinusOneIsInvalid
 //   {
 //       public LibZtSocketHandle() : base(true) { }

        

 //       //[DllImport("libzt", SetLastError = true)]
 //       //private static extern int zts_socket(int domain, int type, int protocol);

 //       //[DllImport("libzt", SetLastError = true)]
 //       //private static extern int zts_close(IntPtr handle);

 //       protected override bool ReleaseHandle()
 //       {
 //           return NativeMethods.zts_bsd_close(handle.ToInt32()) == 0;
 //       }

 //       public static LibZtSocketHandle Create(int domain, int type, int protocol)
 //       {
 //           var handle = new LibZtSocketHandle();
 //           handle.handle = new IntPtr(NativeMethods.zts_bsd_socket(domain, type, protocol));
 //           if (handle.IsInvalid)
 //           {

 //               int error = Marshal.GetLastWin32Error ()    ;
 //               handle.SetHandleAsInvalid();
 //               //throw new SocketException(error);
 //           }
 //           return handle;
 //       }
 //   }

}

