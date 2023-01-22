using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleChat
{
	public partial class ViewController
	{

        ZeroTierNodeManager manager;
        const string networkidstr = "416038decec8db58";
        IPAddress _myIpAddress;
        readonly int _myPort = 50001;
        Server _listener;
        string _myNodeId;
        readonly ConcurrentDictionary<string, Client> _clientList = new ConcurrentDictionary<string, Client>();


        private void OnConnect()
        {
            manager = new ZeroTierNodeManager();
            manager.MessageReceivedEvent += Manager_MessageReceivedEvent;
            manager.NetworkUpdatedEvent += Manager_NetworkUpdatedEvent;
            ulong networkId = (ulong)Int64.Parse(networkidstr, System.Globalization.NumberStyles.HexNumber);
            manager.StartZeroTier(networkId);
            _myNodeId = manager.NodeId;

        }
        private void OnDisconnect()
        {
            ts.Cancel();
            if (_listener != null)
            {
                _listener.OnMessageSending -= Server_OnMessageSending;
                _listener.OnError -= Server_OnError;
                _listener.OnSocketError -= _listener_OnSocketError;
                _listener.Stop();
            }
            if (manager != null)
            {
                manager.MessageReceivedEvent -= Manager_MessageReceivedEvent;
                manager.NetworkUpdatedEvent -= Manager_NetworkUpdatedEvent;
            }
            RemoveAllClients();
            StopUdpListener();
            manager?.StopZeroTier();
            manager = null;
        }
        private void Manager_NetworkUpdatedEvent(object sender, NetworkUpdatedEventArgs e)
        {
            if (e.IPAddresses != null && e.IPAddresses.Any())
            {
                var recvdIp = e.IPAddresses.First();
                if (recvdIp != _myIpAddress)
                {
                    _myIpAddress = e.IPAddresses.First();
                    _myNodeId = manager.NodeId;
                    OnNetworkAuthenticated();
                    
                }
            }
        }
        private void Server_OnError(ErrorMessageEventArgs e)
        {
            ShowErrorAlert(e.Message, "Server error");
        }

        private void Server_OnMessageSending(MessageEventArgs e)
        {
            //AddToChat(e.Message, e.SenderName);
            ShowInfoAlert(e.Message, e.Sender);
        }
        private void _listener_OnSocketError(SocketErrorMessageEventArgs e)
        {
            ShowErrorAlert(e.Message, "Socket server error");
        }

        CancellationTokenSource ts = new CancellationTokenSource();
        private void OnNetworkAuthenticated()
        {
            try
            {
                int myPort = 0;
                InvokeOnMainThread(() =>
                {
                     myPort = Convert.ToInt32(txtMyPort.StringValue);
                });
               if(myPort<= 0)
                {
                    myPort = 50001;
                }
                SetMyIpDetails(_myIpAddress.ToString(), myPort);
                var epLocal = new IPEndPoint(_myIpAddress, myPort);
                if (_listener != null)
                {
                    _listener.OnMessageSending -= Server_OnMessageSending;
                    _listener.OnError -= Server_OnError; ;
                    _listener.OnSocketError -= _listener_OnSocketError;
                    _listener.Stop();
                }
                _listener = new Server(_myIpAddress, myPort, _myNodeId);
                _listener.OnMessageSending += Server_OnMessageSending;
                _listener.OnError += Server_OnError; ;
                _listener.OnSocketError += _listener_OnSocketError;
                 Task.Factory.StartNew(_listener.Start, TaskCreationOptions.LongRunning);


                CancellationToken ct = ts.Token;
                StartUdpListener();
                _ = PeriodicSendAsync(new TimeSpan(0, 0, 30), ct);

                // release button to send message

                InvokeOnMainThread(() =>
                {
                    btnSend.Enabled = true;
                    btnConnect.StringValue = "Connected";
                    btnConnect.Enabled = false;
                    //txtMessage.BecomeFirstResponder();
                });


            }
            catch (Exception ex)
            {
                ShowErrorAlert(ex.Message, "Server event error");
            }
        }
        private void Manager_MessageReceivedEvent(object sender, ZeroTierManagerMessageEventArgs e)
        {
            AddToLog(e.Message, "network");
        }
    }
}

