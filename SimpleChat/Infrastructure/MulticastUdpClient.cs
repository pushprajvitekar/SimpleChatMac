
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZTSockets = ZeroTier.Sockets;

namespace SimpleChat
{
    /// <summary>
    /// Multicast UdpClient wrapper with send and receive capabilities.
    /// Usage: pass local and remote multicast IPs and port to constructor.
    /// Use Send method to send data,
    /// subscribe to Received event to get notified about received data.
    /// </summary>
    public class MulticastUdpClient
    {
        private readonly UdpClient _udpclient;
        private readonly IPAddress _multicastIPaddress;
        private readonly IPAddress _localIPaddress;
        private readonly IPEndPoint _localEndPoint;
        private readonly IPEndPoint _remoteEndPoint;

        public MulticastUdpClient(IPAddress multicastIPaddress, int port, IPAddress localIPaddress = null)
        {
            // Store params
            _multicastIPaddress = multicastIPaddress;
            _localIPaddress = localIPaddress;
            if (localIPaddress == null)
                _localIPaddress = IPAddress.Any;

            // Create endpoints
            _remoteEndPoint = new IPEndPoint(_multicastIPaddress, port);
            _localEndPoint = new IPEndPoint(_localIPaddress, port);

            // Create and configure UdpClient
            _udpclient = new UdpClient();
            // The following three lines allow multiple clients on the same PC
            _udpclient.ExclusiveAddressUse = false;
            _udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpclient.ExclusiveAddressUse = false;
            // Bind, Join
            _udpclient.Client.Bind(_localEndPoint);
            _udpclient.JoinMulticastGroup(_multicastIPaddress, _localIPaddress);

            // Start listening for incoming data
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);

        }
        bool _leaving = false;
        public void LeaveMulticastGroup(string sender)
        {
            var names = sender.Split("@").ToList();
            var nodeid = names.First();
            var ipaddr = names.Last().Split(":").ToList();
            var port = Convert.ToInt32(ipaddr.Last());
            var msg = new UdpPacket() { IpAddress = ipaddr.First(), NodeId = nodeid, Port = port, MessageTypeIdentifier = MessageType.Exit };
            SendMulticast(msg.GetDataStream());
            _leaving = true;
            _udpclient.DropMulticastGroup(_multicastIPaddress);
        }
        /// <summary>
        /// Send the buffer by UDP to multicast address
        /// </summary>
        /// <param name="bufferToSend"></param>
        public void SendMulticast(byte[] bufferToSend)
        {
            _udpclient.Send(bufferToSend, bufferToSend.Length, _remoteEndPoint);
        }

        /// <summary>
        /// Callback which is called when UDP packet is received
        /// </summary>
        /// <param name="ar"></param>
        private void ReceivedCallback(IAsyncResult ar)
        {
            // Get received data
            IPEndPoint sender = new IPEndPoint(0, 0);
            Byte[] receivedBytes = _udpclient.EndReceive(ar, ref sender);

            // fire event if defined
            if (UdpMessageReceived != null)
            {
                var msg = new UdpPacket(receivedBytes);
                if (msg.MessageTypeIdentifier != MessageType.Null)
                {
                    UdpMessageReceived(this, new UdpMessageReceivedEventArgs() { Buffer = receivedBytes, IPAddress = msg.IpAddress, NodeId = msg.NodeId, Port = msg.Port, MessageType = msg.MessageTypeIdentifier });
                }
            }
            if (_leaving)
            {
                _udpclient.Client.Shutdown(SocketShutdown.Both);
                _udpclient.Client.Close();
            }
            else
            {
                // Restart listening for udp data packages
                _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
            }
        }

        /// <summary>
        /// Event handler which will be invoked when UDP message is received
        /// </summary>
        public event EventHandler<UdpMessageReceivedEventArgs> UdpMessageReceived;

        /// <summary>
        /// Arguments for UdpMessageReceived event handler
        /// </summary>
        public class UdpMessageReceivedEventArgs : EventArgs
        {
            public string IPAddress { get; set; }
            public int Port { get; set; }
            public string NodeId { get; set; }
            public string Sender { get; set; }
            public byte[] Buffer { get; set; }

            public MessageType MessageType { get; set; }
        }

    }
}
