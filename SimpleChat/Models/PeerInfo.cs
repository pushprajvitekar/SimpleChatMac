using System;
using System.Net;
using Foundation;

namespace SimpleChat.Models
{
    [Register("PeerInfo")]
    public class PeerInfo:NSObject
	{
		public PeerInfo()
		{
		}
        public int Port { get; set; }
        public string NodeId { get; set; }
		public IPAddress IPAddress { get; set; }
       // private DateTime _date;
        private string _name;
        private string _sender;
        public PeerInfo(string name, IPAddress ipAddress, int port)
        {

            Name = name;
            Sender = ipAddress.ToString();
            Port = port;
            IPAddress = ipAddress;
        }
      

        [Export("Sender")]
        public string Sender
        {
            get { return _sender; }
            set
            {
                WillChangeValue("Sender");
                _sender = value;
                DidChangeValue("Sender");
            }
        }

        [Export("Name")]
        public string Name
        {
            get { return _name; }
            set
            {
                WillChangeValue("Name");
                _name = value;
                DidChangeValue("Name");
            }
        }
    }

    [Register("Peers")]
    public class Peers : NSObject
    {

        private NSMutableArray _peersArray = new NSMutableArray();
        public Peers()
        {
        }
        [Export("peersArray")]
        public NSArray PeersArray
        {
            get { return _peersArray; }
        }
        #region Array Controller Methods
        [Export("addObject:")]
        public void AddPeer(PeerInfo peer)
        {
            WillChangeValue("peersArray");
            _peersArray.Add(peer);
            DidChangeValue("peersArray");
        }

        [Export("insertObject:inpeersArrayAtIndex:")]
        public void InsertPeer(PeerInfo message, nint index)
        {
            WillChangeValue("peersArray");
            _peersArray.Insert(message, index);
            DidChangeValue("peersArray");
        }

        [Export("removeObjectFrompeersArrayAtIndex:")]
        public void RemovePeer(nint index)
        {
            WillChangeValue("peersArray");
            _peersArray.RemoveObject(index);
            DidChangeValue("peersArray");
        }

        [Export("setpeersArray:")]
        public void SetPeer(NSMutableArray array)
        {
            WillChangeValue("peersArray");
            _peersArray = array;
            DidChangeValue("peersArray");
        }

        [Export("removeObjectFrompeersArrayWithName:")]
        public bool RemovePeer(string name)
        {
            WillChangeValue("peersArray");
            var idx = FindIndex(name);
            if (idx > -1)
            {
                RemovePeer((nint)idx);
                DidChangeValue("peersArray");
                return true;
            }
            return false;
            
        }

        public int FindIndex(string name)
        {
            var arr = _peersArray.Filter(NSPredicate.FromFormat("SELF.Name == %@", (NSString)name));
            if (arr.Count > 0)
            {
                var ob = arr.GetItem<PeerInfo>(0);
                var idx = _peersArray.IndexOf(ob);
                return (int)idx;
            }
            return -1;
        }
       
        #endregion
    }
}

