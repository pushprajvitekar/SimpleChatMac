using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ZTSocket = ZeroTier.Sockets.Socket;
using ZTSockets = ZeroTier.Sockets;

namespace SimpleChat
{
    public class Server
    {
        public event MessageEventHandler OnMessageSending;
        public event ErrorEventHandler OnError;
        public event SocketErrorEventHandler OnSocketError;
        private ZTSocket listener;
        public Server(IPAddress localIpAddress, int portNumber, string nodeId)
        {

            LocalIpAddress = localIpAddress;
            PortNumber = portNumber;
            NodeId = nodeId;
            LocalEndPoint = new IPEndPoint(LocalIpAddress, PortNumber);

        }

        public IPEndPoint LocalEndPoint { get; }
        public IPAddress LocalIpAddress { get; }
        public int PortNumber { get; }
        public string NodeId { get; }



        bool runLoop = true;
        public void Start()
        {


            // Bind the socket to the local endpoint and
            // listen for incoming connections.

            try
            {
                listener = new ZTSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Blocking = false;
                // listener.LingerState = new LingerOption(true, 10);

                listener.Bind(LocalEndPoint);
                listener.Listen(100);
                while (runLoop)
                {
                    //if (Poll(5000))
                    //{
                    try
                    {
                        var xclient = listener.Accept();
                        HandleClient(xclient);
                    }
                    catch //(ZTSockets.SocketException)
                    {
                        // OnSocketError?.Invoke(new SocketErrorMessageEventArgs($"Error: {e.Message}", LocalIpAddress, e.ServiceErrorCode, e.SocketErrorCode));
                    }

                    //}
                }

            }
            //catch (ZTSockets.SocketException e)
            //{
            //    OnSocketError?.Invoke(new SocketErrorMessageEventArgs($"Error: {e.Message}", LocalIpAddress, e.ServiceErrorCode, e.SocketErrorCode));
            //}
            catch (Exception ex)
            {
                OnError?.Invoke(new ErrorMessageEventArgs($"Error: {ex.Message}", LocalIpAddress));
            }
        }

        private bool Poll(int ms)
        {
            try
            {
                return listener.Poll(ms, SelectMode.SelectRead);
            }
            catch (ZTSockets.SocketException)
            {
                return false;
            }
        }
        private static readonly object socketLock = new object();
        public void Stop()
        {
            if (listener != null)
            {
                try
                {
                    lock (socketLock)
                    {
                        runLoop = false;
                        Thread.Sleep(1000);
                        listener.Shutdown(SocketShutdown.Both);
                        listener.Close();
                    }
                }
                catch
                {

                }
            }
        }
        private static readonly object clientsocketLock = new object();
        private void HandleClient(ZTSocket acceptedClient)
        {
            var clientEndpoint = (IPEndPoint)acceptedClient.RemoteEndPoint;
            var clientAddress = IPAddress.Parse(clientEndpoint.Address.ToString());
            string data = string.Empty;
            //var ackMesPacket = new MessagePacket() { MessageTypeIdentifier = MessageType.Ack, ChatName = NodeId };
            //var acKMsg = ackMesPacket.GetDataStream();
            var packet = new MessagePacket();
            try
            {
                try
                {
                    lock (clientsocketLock)
                    {
                        acceptedClient.ReceiveTimeout = 3000;
                        acceptedClient.ConnectTimeout = 3000;

                        packet = acceptedClient.ReceiveMessagePacket();
                        //acceptedClient.Send(acKMsg, 0, acKMsg.Length, SocketFlags.None);

                        acceptedClient.Shutdown(SocketShutdown.Both);
                        acceptedClient.Close();

                    }
                }
                catch (ZTSockets.SocketException e)
                {
                    OnSocketError?.Invoke(new SocketErrorMessageEventArgs($"Error: {e.Message}", clientAddress, e.ServiceErrorCode, e.SocketErrorCode));
                }
                catch { }
                if (packet.MessageTypeIdentifier != MessageType.Null)
                {

                    OnMessageSending?.Invoke(new MessageEventArgs(packet.ChatMessage, clientAddress, $"{packet.ChatName}"));
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(new ErrorMessageEventArgs($"Error: {ex.Message}", clientAddress));
            }
        }


    }
}
