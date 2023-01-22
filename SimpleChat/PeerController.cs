using System;
using SimpleChat.Models;
using System.Net;
using Foundation;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleChat
{
	public partial class ViewController
	{
       

        private void AddItemToPeerList(string nodeId, IPAddress ipAddress, int port)
        {
            InvokeOnMainThread(() =>
            {
                if (PeerList.FindIndex(nodeId) < 0)
                {
                    PeerList.AddPeer(new PeerInfo(nodeId, ipAddress, port));
                    AddToChat($"{nodeId} joined the chat.", "system");
                }
            }
            );
        }

        private void RemoveItemToPeerList(string nodeId, IPAddress iPAddress, int port)
        {
            InvokeOnMainThread(() =>
            {
                RemoveClient(nodeId);
              
                if (PeerList.RemovePeer(nodeId))
                {
                    AddToChat($"{nodeId} left the chat.", "system");
                }
                
            }
             );
        }

        /// <summary>
        /// UDP Message received event
        /// </summary>
        void OnUdpMessageReceived(object sender, MulticastUdpClient.UdpMessageReceivedEventArgs e)
        {
            if (e != null && e.MessageType != MessageType.Null)
            {
                var senderip = IPAddress.Parse(e.IPAddress);
                var senderNodeID = e.NodeId;
                if (senderip == _myIpAddress || senderNodeID == _myNodeId) return;
                switch (e.MessageType)
                {

                    case MessageType.Enter:
                        AddItemToPeerList(senderNodeID, senderip, e.Port);
                        break;
                    case MessageType.Exit:
                        RemoveItemToPeerList(senderNodeID, senderip, e.Port);
                        break;
                    default: break;
                }
            }
        }

        MulticastUdpClient _udpClientWrapper;
        const int udpPort = 2222;
        const string udpAddress = "239.0.0.222";
        private void StartUdpListener()
        {
            // Create address objects
            IPAddress multicastIPaddress = IPAddress.Parse(udpAddress);
            IPAddress localIPaddress = IPAddress.Any;

            // Create MulticastUdpClient
            _udpClientWrapper = new MulticastUdpClient(multicastIPaddress, udpPort, localIPaddress);
            _udpClientWrapper.UdpMessageReceived += OnUdpMessageReceived;
            AddToLog($"UDP Client started",_myIpAddress.ToString());
        }
        private void StopUdpListener()
        {
            if (_udpClientWrapper != null)
            {

                _udpClientWrapper.LeaveMulticastGroup($"{_myNodeId}@{_myIpAddress}:{_myPort}");
                _udpClientWrapper.UdpMessageReceived -= OnUdpMessageReceived;
            }
        }
        //private async Task RunInBackground(CancellationToken ct)
        //{
        //    var periodicTime = new PeriodicTimer(TimeSpan.FromSeconds(5));
        //    while (await periodicTime.WaitForNextTickAsync(ct))
        //    {
        //        SendAddMeMessage();
        //    }
        //}
        public async Task PeriodicSendAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                await SendAddMeMessage();
                await Task.Delay(interval, cancellationToken);
            }
        }
        private async Task SendAddMeMessage()
        {
            var msg = new UdpPacket() { IpAddress = _myIpAddress.ToString(), NodeId = _myNodeId, Port = _myPort, MessageTypeIdentifier = MessageType.Enter };
            // Send
            await Task.Run(()=>_udpClientWrapper.SendMulticast(msg.GetDataStream()));
            //DisplayClientMessage("Sent message: " + $"{msg.ChatMessage}", $"{_myNodeId}@{_myIpAddress}");
        }

    }
}

