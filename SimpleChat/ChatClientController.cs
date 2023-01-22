
using System;
using SimpleChat.Models;
using System.Net;
using System.Threading.Tasks;
using ZeroTier.Core;

namespace SimpleChat
{
    public partial class ViewController
    {

        private async Task OnSendClick(Models.PeerInfo peer)
        {
            try
            {

                if (peer == null)
                {
                    ShowInfoAlert("Select any friend to send message", "Send");
                    return;
                }
                var remoteAddress = peer.IPAddress;
                var remotePort = peer.Port;

                var client = CreateClient(peer.Name, remoteAddress, remotePort);

                var message = txtMessage.StringValue;

                if (!string.IsNullOrEmpty(message))
                {
                  await Task.Run(() => client.Send(message));
                  AddToChat(message, $"[{_myNodeId}]");
                  txtMessage.StringValue = string.Empty; ;
                }
            }
            catch (Exception ex)
            {
                ShowErrorAlert(ex.ToString(), "Send Error");
            }
        }

        private Client CreateClient(string nodeId, IPAddress remoteAddress, int remotePort)
        {

            if (!_clientList.TryGetValue(nodeId, out Client client))
            {
                client = new Client(remoteAddress, remotePort, _myNodeId);
                client.OnError += Client_OnError;
                client.OnSocketError += _client_OnSocketError;
                client.OnMessageSending += _client_OnMessageSending;
                _clientList.TryAdd(nodeId, client);
            }
            return client;
        }

        private void RemoveAllClients()
        {
            foreach (var nodeId in _clientList.Keys)
            {
                RemoveClient(nodeId);
            }
        }
        private void RemoveClient(string nodeId)
        {
            if (_clientList.TryRemove(nodeId, out Client client))
            {
                if (client != null)
                {
                    client.OnError -= Client_OnError;
                    client.OnMessageSending -= _client_OnMessageSending;
                    client.OnSocketError -= _client_OnSocketError;
                    client.Disconnect();
                }
            }
        }
        private void _client_OnSocketError(SocketErrorMessageEventArgs e)
        {
            ShowErrorAlert(e.ToString(), "Socket Server Error");
        }

        private void _client_OnMessageSending(MessageEventArgs e)
        {
            //AddToChat(e.Message, e.Sender);
            //ShowInfoAlert(e.Message, e.Sender);
        }

        private void Client_OnError(ErrorMessageEventArgs e)
        {
            ShowErrorAlert(e.Message, "Send client Error");
        }
    }
}

