using System;
using Foundation;

namespace SimpleChat.Models
{
    [Register("LogMessage")]
    public class LogMessage : NSObject
	{
		public LogMessage()
		{
		}
        private DateTime _date;
        private string _message;
        private string _sender;
        public LogMessage(string sender, string message)
        {
            EntryDate = DateTime.Now.ToString();
            Sender = sender;
            Message = message;

        }
        [Export("Date")]
        public string EntryDate
        {
            get { return _date.ToShortTimeString(); }
            set
            {
                WillChangeValue("Date");
                _date = Convert.ToDateTime(value);
                DidChangeValue("Date");
            }
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

        [Export("Message")]
        public string Message
        {
            get { return _message; }
            set
            {
                WillChangeValue("Message");
                _message = value;
                DidChangeValue("Message");
            }
        }

    }

    [Register("LogMessages")]
    public class LogMessages : NSObject
    {

        private NSMutableArray _messages = new NSMutableArray();
        public LogMessages()
        {
        }
        [Export("logMessagesArray")]
        public NSArray Messages
        {
            get { return _messages; }
        }
        #region Array Controller Methods
        [Export("addObject:")]
        public void AddMessage(LogMessage message)
        {
            WillChangeValue("logMessagesArray");
            _messages.Add(message);
            DidChangeValue("logMessagesArray");
        }

        [Export("insertObject:inLogMessagesArrayAtIndex:")]
        public void InsertMessage(LogMessage message, nint index)
        {
            WillChangeValue("logMessagesArray");
            _messages.Insert(message, index);
            DidChangeValue("logMessagesArray");
        }

        [Export("removeObjectFromLogMessagesArrayAtIndex:")]
        public void RemoveMessage(nint index)
        {
            WillChangeValue("logMessagesArray");
            _messages.RemoveObject(index);
            DidChangeValue("logMessagesArray");
        }

        [Export("setLogMessagesArray:")]
        public void SetMessage(NSMutableArray array)
        {
            WillChangeValue("logMessagesArray");
            _messages = array;
            DidChangeValue("logMessagesArray");
        }
        #endregion
    }
}

