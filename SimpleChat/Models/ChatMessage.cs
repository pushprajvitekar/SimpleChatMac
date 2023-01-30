using System;
using Foundation;

namespace SimpleChat.Models
{
    [Register("ChatMessage")]
    public class ChatMessage :NSObject
	{

        private DateTime _date;
        private string _message;
        private string _sender;
		public ChatMessage(string sender, string message)
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
        public string Sender {
            get { return _sender; }
            set
            {
                WillChangeValue("Sender");
                _sender = value;
                DidChangeValue("Sender");
            }
        }

        [Export("Message")]
        public string Message {
            get { return _message; }
            set
            {
                WillChangeValue("Message");
                _message = value;
                DidChangeValue("Message");
            }
        }
	}
}

