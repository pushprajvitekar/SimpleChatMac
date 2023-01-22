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
        private readonly ZTSocket listener;
        public Server(IPAddress localIpAddress, int portNumber, string nodeId)
        {

            LocalIpAddress = localIpAddress;
            PortNumber = portNumber;
            NodeId = nodeId;
            LocalEndPoint = new IPEndPoint(LocalIpAddress, PortNumber);
            listener = new ZTSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public IPEndPoint LocalEndPoint { get; }
        public IPAddress LocalIpAddress { get; }
        public int PortNumber { get; }
        public string NodeId { get; }



        bool runLoop = true;
        public async void Start()
        {


            // Bind the socket to the local endpoint and
            // listen for incoming connections.

            try
            {
                listener.Bind(LocalEndPoint);
                listener.Listen(100);
                while (runLoop)
                {
                    var xclient = listener.Accept();
                    await Task.Run(() => HandleClient(xclient));
                    //task.Wait();
                }

            }
            catch (ZTSockets.SocketException e)
            {
                OnSocketError?.Invoke(new SocketErrorMessageEventArgs($"Error: {e.Message}", LocalIpAddress, e.ServiceErrorCode, e.SocketErrorCode));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(new ErrorMessageEventArgs($"Error: {ex.Message}", LocalIpAddress));
            }
        }
        public void Stop()
        {
            if (listener != null)
            {
                runLoop = false;
                Thread.Sleep(1000);
                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
        }
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
                    packet = acceptedClient.ReceiveMessagePacket();
                    //acceptedClient.Send(acKMsg, 0, acKMsg.Length, SocketFlags.None);
                    acceptedClient.Shutdown(SocketShutdown.Both);
                    acceptedClient.Close();
                }
                catch (ZTSockets.SocketException e)
                {
                    OnSocketError?.Invoke(new SocketErrorMessageEventArgs($"Error: {e.Message}" ,clientAddress,e.ServiceErrorCode, e.SocketErrorCode));
                }
                if (packet.MessageTypeIdentifier != MessageType.Null)
                {

                    OnMessageSending?.Invoke(new MessageEventArgs(packet.ChatMessage,clientAddress, $"{packet.ChatName}"));
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(new ErrorMessageEventArgs($"Error: {ex.Message}", clientAddress));
            }
        }


    }
}
