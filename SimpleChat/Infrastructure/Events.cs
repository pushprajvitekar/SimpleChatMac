using System;
using System.Net;

namespace SimpleChat
{
    public class ErrorMessageEventArgs : EventArgs
    {
        public string Message { get; protected set; }
        public string Sender => SenderIpAddress != null ? SenderIpAddress.ToString() : string.Empty;
        public IPAddress SenderIpAddress { get; protected set; }
        public ErrorMessageEventArgs(string message, IPAddress sender)
        {
            Message = message;
            SenderIpAddress = sender;
        }
    }

    public delegate void ErrorEventHandler(ErrorMessageEventArgs e);

    public class SocketErrorMessageEventArgs : ErrorMessageEventArgs
    {

        public int SocketErrorCode { get; protected set; }
        public int ServiceErrorCode { get; protected set; }
        public SocketErrorMessageEventArgs(string message, IPAddress sender, int serviceErrorCode, int socketErrorCode) : base(message, sender)
        {
            ServiceErrorCode = serviceErrorCode;
            SocketErrorCode = socketErrorCode;
            Message = $"{Message}, Service error code: {ServiceErrorCode}, Socket error code: {SocketErrorCode}";
        }
    }
    public delegate void SocketErrorEventHandler(SocketErrorMessageEventArgs e);
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; protected set; }
        public string Sender => SenderIpAddress != null ? SenderIpAddress.ToString() : string.Empty;
        public IPAddress SenderIpAddress { get; protected set; }
        public MessageType MessageType { get; }
        public string NodeId { get; set; }
        public string SenderName => $"{NodeId}";
        public MessageEventArgs(string message, IPAddress sender, string nodeId)
        {
            Message = message;
            SenderIpAddress = sender;
            NodeId = nodeId;
        }
    }
    public delegate void MessageEventHandler(MessageEventArgs e);
}
