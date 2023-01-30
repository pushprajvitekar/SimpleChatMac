using System;
using System.Collections.Generic;
using AppKit;
using Darwin;
using Foundation;
using SimpleChat.Models;
using StoreKit;

namespace SimpleChat
{
    public partial class ViewController : NSViewController
    {


        #region cocoa bindings
        ChatMessages _messages;
        [Export("Messages")]
        public ChatMessages Messages
        {
            get { return _messages; }
            set
            {
                WillChangeValue("Messages");
                _messages = value;
                DidChangeValue("Messages");
            }
        }
        LogMessages _logmessages;
        [Export("Logs")]
        public LogMessages Logs
        {
            get { return _logmessages; }
            set
            {
                WillChangeValue("Logs");
                _logmessages = value;
                DidChangeValue("Logs");
            }
        }
        Peers _peerList;
        [Export("PeerList")]
        public Peers PeerList
        {
            get { return _peerList; }
            set
            {
                WillChangeValue("PeerList");
                _peerList = value;
                DidChangeValue("PeerList");
            }
        }
        #endregion cocoa bindings
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            Logs = new LogMessages();
            PeerList = new Peers();
            Messages = new ChatMessages();
            txtMyPort.StringValue = "50001";
            tblViewFriends.SelectionDidChange += TblViewFriends_SelectionDidChange;
            // PeerList.AddPeer(new PeerInfo("Jenny", System.Net.IPAddress.Parse("10.10.12.58"), 50001));
            NSWindow.Notifications.ObserveWillClose(new EventHandler<NSNotificationEventArgs>(OnWindowWillClose));
        }
        private void OnWindowWillClose(object sender, NSNotificationEventArgs eventArgs)
        {
            OnDisconnect();
        }


        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
        #region ui events
        partial void btnConnect_Click(NSObject sender)
        {

            //Messages.InsertMessage(new ChatMessage("Neil", "I am a pid"), 0);
            //Logs.AddMessage(new LogMessage("Yash", "I am a cry baby"));
            //PeerList.AddPeer(new PeerInfo("Kim", System.Net.IPAddress.Parse("10.19.23.55")));
            OnConnect();


        }
        partial  void btnSend_Click(NSObject sender)
        {
            //Messages.RemoveMessage(0);
            //Logs.RemoveMessage(0);
            //PeerList.RemovePeer(0);
            try
            {
                if (tblViewFriends.SelectedRowCount > 0)
                {
                    var selectedPerson = PeerList.PeersArray.GetItem<PeerInfo>((nuint)tblViewFriends.SelectedRow);
                    OnSendClick(selectedPerson);
                }
            }
            catch (Exception ex)
            {
                ShowErrorAlert(ex.Message, "");
            }

        }

        #endregion ui events
        private void SetPeerIpDetails(string ipaddress, int portNum)
        {
            InvokeOnMainThread(() =>
            {
                txtFriendIpAddress.StringValue = ipaddress;
                txtFriendPort.IntValue = portNum;
            });
        }
        private void SetMyIpDetails(string ipaddress, int portNum)
        {
            InvokeOnMainThread(() =>
            {
                txtMyIpAddress.StringValue = ipaddress;
                txtMyPort.StringValue = Convert.ToString(portNum);
            });
        }
        private void AddToLog(string message, string sender)
        {
            InvokeOnMainThread(() =>
            {
                Logs.InsertMessage(new LogMessage(sender, message), 0);
            });
        }
        private void AddToChat(string message, string sender)
        {
            InvokeOnMainThread(() =>
            {
                Messages.InsertMessage(new ChatMessage(sender, message), 0);
            });
        }
        private void ShowErrorAlert(string message, string title)
        {
            InvokeOnMainThread(() =>
            {
                var alert = new NSAlert();
                alert.AlertStyle = NSAlertStyle.Critical;
                alert.MessageText = title;
                alert.InformativeText = message;
                alert.RunModal();
            });

        }
        private void ShowInfoAlert(string message, string title)
        {
            if (!string.IsNullOrEmpty(message))
            {
                InvokeOnMainThread(() =>
                {
                    var alert = new NSAlert();
                    alert.AlertStyle = NSAlertStyle.Informational;
                    alert.MessageText = title;
                    alert.InformativeText = message;
                    alert.RunModal();
                });
            }

        }
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            //         Messages = new ChatMessages();
            //Messages.AddMessage(new ChatMessage("John", "This is ridiculous"));
            //         Messages.AddMessage(new ChatMessage("Mary", "No it is not"));
            //         Messages.AddMessage(new ChatMessage("Ram", "I am so not in for this"));
            //         Messages.AddMessage(new ChatMessage("Ali", "Hell yeah I am in"));
            //         Messages.AddMessage(new ChatMessage("Garry", "Drinks are on me"));

            //         Logs = new LogMessages();
            //         Logs.AddMessage(new LogMessage("John", "This is ridiculous"));
            //         Logs.AddMessage(new LogMessage("Mary", "No it is not"));
            //         Logs.AddMessage(new LogMessage("Ram", "I am so not in for this"));
            //         Logs.AddMessage(new LogMessage("Ali", "Hell yeah I am in"));
            //         Logs.AddMessage(new LogMessage("Garry", "Drinks are on me"));

            //PeerList = new Peers();

            //PeerList.AddPeer(new PeerInfo("Harry", System.Net.IPAddress.Parse("10.19.23.45")));
            //PeerList.AddPeer(new PeerInfo("Sally", System.Net.IPAddress.Parse("10.19.23.46")));
            //PeerList.AddPeer(new PeerInfo("Tom", System.Net.IPAddress.Parse("10.19.23.5")));
            //PeerList.AddPeer(new PeerInfo("Joey", System.Net.IPAddress.Parse("10.19.23.88")));
        }

        private void TblViewFriends_SelectionDidChange(object sender, EventArgs e)
        {
            var selectedPerson = PeerList.PeersArray.GetItem<PeerInfo>((nuint)tblViewFriends.SelectedRow);
            if (selectedPerson != null)
            {
                txtFriendIpAddress.StringValue = selectedPerson.IPAddress.ToString();
                txtFriendPort.StringValue = Convert.ToString(selectedPerson.Port);
            }
        }
    }
}
