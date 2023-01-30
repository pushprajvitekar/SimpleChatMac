using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SimpleChat
{
    public enum MessageType
    {
        Message,
        Enter,
        Exit,
        Ack,
        Null
    }

    // ----------------
    // Packet Structure
    // ----------------

    // Description   -> |dataIdentifier|name length|message length|    name   |    message   |:EOM
    // Size in bytes -> |       4      |     4     |       4      |name length|message length| 4
    public class MessagePacket
    {
        #region Private Members
        #endregion

        #region Public Properties
        public MessageType MessageTypeIdentifier { get; set; }

        public string ChatName { get; set; }

        public string ChatMessage { get; set; }
        #endregion

        #region Methods

        // Default Constructor
        public MessagePacket()
        {
            MessageTypeIdentifier = MessageType.Null;
        }

        public MessagePacket(byte[] dataStream)
        {
            if (dataStream != null && dataStream.Length >= 4)
            {
                // Read the data identifier from the beginning of the stream (4 bytes)
                MessageTypeIdentifier = (MessageType)BitConverter.ToInt32(dataStream, 0);

                // Read the length of the name (4 bytes)
                int nameLength = BitConverter.ToInt32(dataStream, 4);

                // Read the length of the message (4 bytes)
                int msgLength = BitConverter.ToInt32(dataStream, 8);

                // Read the name field
                if (nameLength > 0)
                    ChatName = Encoding.UTF8.GetString(dataStream, 12, nameLength);
                else
                    ChatName = string.Empty;

                // Read the message field
                if (msgLength > 0)
                    ChatMessage = Encoding.UTF8.GetString(dataStream, 12 + nameLength, msgLength);
                else
                    ChatMessage = string.Empty;
            }
            else
            {

            }
        }

        // Converts the packet into a byte array for sending/receiving 
        public byte[] GetDataStream()
        {
            List<byte> dataStream = new List<byte>();
            try
            {




                // Add the dataIdentifier
                dataStream.AddRange(BitConverter.GetBytes((int)MessageTypeIdentifier));

                // Add the name length
                if (ChatName != null)
                    dataStream.AddRange(BitConverter.GetBytes(ChatName.Length));
                else
                    dataStream.AddRange(BitConverter.GetBytes(0));

                // Add the message length
                if (ChatMessage != null)
                    dataStream.AddRange(BitConverter.GetBytes(ChatMessage.Length));
                else
                    dataStream.AddRange(BitConverter.GetBytes(0));

                // Add the name
                if (ChatName != null)
                    dataStream.AddRange(Encoding.UTF8.GetBytes(ChatName));

                // Add the message
                if (ChatMessage != null)
                    dataStream.AddRange(Encoding.UTF8.GetBytes(ChatMessage));

                dataStream.AddRange(Encoding.UTF8.GetBytes(":EOM"));
                
            }
            catch (Exception )
            {

            }
            return dataStream.ToArray();
        }

        #endregion
    }

    // ----------------
    // Packet Structure
    // ----------------

    // Description   -> |dataIdentifier|name length|message length|    name   |    message   |Port|:EOM
    // Size in bytes -> |       4      |     4     |       4      |name length|message length|  4 |   4
    public class UdpPacket
    {
        #region Private Members
        #endregion

        #region Public Properties
        public MessageType MessageTypeIdentifier { get; set; }

        public string NodeId { get; set; }

        public string IpAddress { get; set; }

        public int Port { get; set; }
        #endregion

        #region Methods

        // Default Constructor
        public UdpPacket()
        {
            MessageTypeIdentifier = MessageType.Null;
        }

        public UdpPacket(byte[] dataStream)
        {
            if (dataStream != null && dataStream.Length >= 4)
            {
                // Read the data identifier from the beginning of the stream (4 bytes)
                MessageTypeIdentifier = (MessageType)BitConverter.ToInt32(dataStream, 0);

                // Read the length of the name (4 bytes)
                int nameLength = BitConverter.ToInt32(dataStream, 4);

                // Read the length of the message (4 bytes)
                int msgLength = BitConverter.ToInt32(dataStream, 8);

                // Read the name field
                if (nameLength > 0)
                    NodeId = Encoding.UTF8.GetString(dataStream, 12, nameLength);
                else
                    NodeId = string.Empty;

                // Read the message field
                if (msgLength > 0)
                    IpAddress = Encoding.UTF8.GetString(dataStream, 12 + nameLength, msgLength);
                else
                    IpAddress = string.Empty;

                Port = BitConverter.ToInt32(dataStream, 12 + nameLength + msgLength);
            }
            else
            {

            }
        }

        // Converts the packet into a byte array for sending/receiving 
        public byte[] GetDataStream()
        {
            List<byte> dataStream = new List<byte>();

            // Add the dataIdentifier
            dataStream.AddRange(BitConverter.GetBytes((int)MessageTypeIdentifier));

            // Add the name length
            if (NodeId != null)
                dataStream.AddRange(BitConverter.GetBytes(NodeId.Length));
            else
                dataStream.AddRange(BitConverter.GetBytes(0));

            // Add the message length
            if (IpAddress != null)
                dataStream.AddRange(BitConverter.GetBytes(IpAddress.Length));
            else
                dataStream.AddRange(BitConverter.GetBytes(0));

            // Add the name
            if (NodeId != null)
                dataStream.AddRange(Encoding.UTF8.GetBytes(NodeId));

            // Add the message
            if (IpAddress != null)
                dataStream.AddRange(Encoding.UTF8.GetBytes(IpAddress));

            dataStream.AddRange(BitConverter.GetBytes(Port));
            dataStream.AddRange(Encoding.UTF8.GetBytes(":EOM"));
            return dataStream.ToArray();
        }

        #endregion
    }
}
