using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroTier.Core;

namespace SimpleChat
{
    public class ZeroTierManagerMessageEventArgs : EventArgs
    {

        public ZeroTierManagerMessageEventArgs(string message) : base()
        {
            Message = message;
        }

        public string Message { get; }
    }
    public class NetworkUpdatedEventArgs : EventArgs
    {

        public NetworkUpdatedEventArgs() : base()
        {
        }
        public string NodeId { get; set; }
        public List<IPAddress> IPAddresses { get; set; } = new List<IPAddress>();
        public List<PeerInfo> Peers { get; set; } = new List<PeerInfo>();

        public List<RouteInfo> Routes { get; set; } = new List<RouteInfo>();
    }
    public class ZeroTierNodeManager
    {
        // ZeroTier Node instance

         Node node;
        ulong _networkId = 0;
        public delegate void ZeroTierManagerMessageHandler(object sender, ZeroTierManagerMessageEventArgs e);
        public event ZeroTierManagerMessageHandler MessageReceivedEvent;
        public delegate void NetworkUpdatedEventHandler(object sender, NetworkUpdatedEventArgs e);
        public event NetworkUpdatedEventHandler NetworkUpdatedEvent;
        public ZeroTierNodeManager()
        {
            node = new Node();
        }
        // Initialize and start ZeroTier
        public string NodeId { get { return node?.Id.ToString("x16") ?? string.Empty; } }

        public List<IPAddress> IpAdresses { get { return node.GetNetworkAddresses(_networkId); } }
        public void StartZeroTier(ulong networkId)
        {

            _networkId = networkId;
            // (OPTIONAL) Initialize node

            //node.InitFromStorage(configFilePath);
            //node.InitAllowNetworkCaching(false);
            //node.InitAllowPeerCaching(true);
            // node.InitAllowIdentityCaching(true);
            // node.InitAllowWorldCaching(false);
            node.InitSetEventHandler(OnZeroTierEvent);
            // node.InitSetPort(0);   // Will randomly attempt ports if not specified or is set to 0
            node.InitSetRandomPortRange(40000, 50000);
            // node.InitAllowSecondaryPort(false);

            // (OPTIONAL) Set custom signed roots

            // In this case we only allow ZeroTier to contact our Amsterdam root server
            // To see examples of how to generate and sign roots definitions see docs.zerotier.com

            /*
            var rootsData = new byte[] {
                0x01, 0x00, 0x00, 0x00, 0x00, 0x08, 0xea, 0xc9, 0x0a, 0x00, 0x00, 0x01, 0x6c, 0xe3, 0xe2, 0x39, 0x55, 0x74,
                0xeb, 0x27, 0x9d, 0xc9, 0xe7, 0x5a, 0x52, 0xbb, 0x91, 0x8f, 0xf7, 0x43, 0x3c, 0xbf, 0x77, 0x5a, 0x4b, 0x57,
                0xb4, 0xe1, 0xe9, 0xa1, 0x01, 0x61, 0x3d, 0x25, 0x35, 0x60, 0xcb, 0xe3, 0x30, 0x18, 0x1e, 0x6e, 0x44, 0xef,
                0x93, 0x89, 0xa0, 0x19, 0xb8, 0x7b, 0x36, 0x0b, 0x92, 0xff, 0x0f, 0x1b, 0xbe, 0x56, 0x5a, 0x46, 0x91, 0x36,
                0xf1, 0xd4, 0x5c, 0x09, 0x05, 0xe5, 0xf5, 0xfb, 0xba, 0xe8, 0x13, 0x2d, 0x47, 0xa8, 0xe4, 0x1b, 0xa5, 0x1c,
                0xcf, 0xb0, 0x2f, 0x27, 0x7e, 0x95, 0xa0, 0xdd, 0x49, 0xe1, 0x7d, 0xc0, 0x7e, 0x6d, 0xe3, 0x25, 0x91, 0x96,
                0xc2, 0x55, 0xf9, 0x20, 0x6d, 0x2a, 0x5e, 0x1b, 0x41, 0xcb, 0x1f, 0x8d, 0x57, 0x27, 0x69, 0x3e, 0xcc, 0x7f,
                0x0b, 0x36, 0x54, 0x6b, 0xd3, 0x80, 0x78, 0xf6, 0xd0, 0xec, 0xb4, 0x31, 0x6b, 0x87, 0x1b, 0x50, 0x08, 0xe4,
                0x0b, 0xa9, 0xd4, 0xfd, 0x37, 0x79, 0x14, 0x6a, 0xf5, 0x12, 0xf2, 0x45, 0x39, 0xca, 0x23, 0x00, 0x39, 0xbc,
                0xa3, 0x1e, 0xa8, 0x4e, 0x23, 0x2d, 0xc8, 0xdb, 0x9b, 0x0e, 0x52, 0x1b, 0x8d, 0x02, 0x72, 0x01, 0x99, 0x2f,
                0xcf, 0x1d, 0xb7, 0x00, 0x20, 0x6e, 0xd5, 0x93, 0x50, 0xb3, 0x19, 0x16, 0xf7, 0x49, 0xa1, 0xf8, 0x5d, 0xff,
                0xb3, 0xa8, 0x78, 0x7d, 0xcb, 0xf8, 0x3b, 0x8c, 0x6e, 0x94, 0x48, 0xd4, 0xe3, 0xea, 0x0e, 0x33, 0x69, 0x30,
                0x1b, 0xe7, 0x16, 0xc3, 0x60, 0x93, 0x44, 0xa9, 0xd1, 0x53, 0x38, 0x50, 0xfb, 0x44, 0x60, 0xc5, 0x0a, 0xf4,
                0x33, 0x22, 0xbc, 0xfc, 0x8e, 0x13, 0xd3, 0x30, 0x1a, 0x1f, 0x10, 0x03, 0xce, 0xb6, 0x00, 0x02, 0x04, 0xc3,
                0xb5, 0xad, 0x9f, 0x27, 0x09, 0x06, 0x2a, 0x02, 0x6e, 0xa0, 0xc0, 0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x27, 0x09
            };
            node.InitSetRoots(rootsData, rootsData.Length);
            */

            node.Start();   // Network activity only begins after calling Start()


            //NodeDetailsUpdate();

            //JoinNetwork(networkId);

        }

        private void JoinNetwork(ulong networkId)
        {
            node.Join(networkId);
            SendToMessageList("Waiting for authorisation from network controller to complete...");

        }

        private void UpdateNodeNetwork(ulong networkId)
        {
            if (_networkId == networkId)
            {
                if (node.Networks.Count > 0 && node.IsNetworkTransportReady(networkId))
                {
                    var addresses = node.GetNetworkAddresses(networkId);
                    SendToMessageList("Num of assigned addresses : " + addresses.Count);
                    foreach (IPAddress addr in addresses)
                    {
                        SendToMessageList(" - Address: " + addr);
                    }
                    var routes = node.GetNetworkRoutes(networkId);
                    SendToMessageList("Num of routes             : " + routes.Count);
                    foreach (RouteInfo route in routes)
                    {
                        SendToMessageList(
                            $" -   Route: target={route.Target} via={route.Via} flags={route.Flags} metric={route.Metric}");
                    }

                    var peers = node.Peers;
                    SendNetworkUpdateEvent(addresses, routes, peers);
                }
                else
                {
                    // Wait until we've joined the network and we have routes + addresses
                    SendToMessageList("Waiting for network to become transport ready...");
                }
            }

        }

        private void NodeDetailsUpdate()
        {
            SendToMessageList("Id            : " + node.IdString);
            SendToMessageList("Version       : " + node.Version);
            SendToMessageList("PrimaryPort   : " + node.PrimaryPort);
            SendToMessageList("SecondaryPort : " + node.SecondaryPort);
            SendToMessageList("TertiaryPort  : " + node.TertiaryPort);
        }

        /**
         * Stop ZeroTier
         */
        public void StopZeroTier()
        {
            node.Stop();
            node.Free();
            node.Leave(_networkId);
            node = null;
        }

        /**
         * (OPTIONAL)
         *
         * Your application should process event messages and return control as soon as possible. Blocking
         * or otherwise time-consuming operations are not recommended here.
         */
        public void OnZeroTierEvent(Event e)
        {
            // SendToMessageList($"Event.Code = {e.Code} ({e.Name})");

            if (e.Code == ZeroTier.Constants.EVENT_NODE_ONLINE)
            {
                SendToMessageList("Node is online");
                SendToMessageList(" - Address (NodeId): " + node.Id.ToString("x16"));
                NodeDetailsUpdate();
                JoinNetwork(_networkId);

            }
            if (e.Code == ZeroTier.Constants.EVENT_NETWORK_UPDATE)
            {
                SendToMessageList(" - Network ID: " + e.NetworkInfo.Id.ToString("x16"));
                UpdateNodeNetwork(e.NetworkInfo.Id);

            }
            if (e.Code == ZeroTier.Constants.EVENT_NETWORK_OK)
            {
                SendToMessageList(" - Network ID: " + e.NetworkInfo.Id.ToString("x16"));
            }


        }
        protected virtual void SendToMessageList(string m)
        {
            MessageReceivedEvent?.Invoke(this, new ZeroTierManagerMessageEventArgs(m));
        }

        protected virtual void SendNetworkUpdateEvent(List<IPAddress> ipAddresses, List<RouteInfo> routes, List<PeerInfo> peers)
        {
            NetworkUpdatedEvent?.Invoke(this, new NetworkUpdatedEventArgs() { IPAddresses = ipAddresses, Routes = routes, Peers = peers });
        }
    }
}
