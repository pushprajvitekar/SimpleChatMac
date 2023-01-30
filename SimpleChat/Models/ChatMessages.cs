using System;
using CoreBluetooth;
using Foundation;

namespace SimpleChat.Models
{
    [Register("ChatMessages")]
    public class ChatMessages :NSObject
	{

        private NSMutableArray _messages = new NSMutableArray();
        public ChatMessages()
		{
		}
        [Export("chatMessagesArray")]
        public NSArray Messages
        {
            get { return _messages; }
        }
        #region Array Controller Methods
        [Export("addObject:")]
        public void AddMessage(ChatMessage message)
        {
            WillChangeValue("chatMessagesArray");
            _messages.Add(message);
            DidChangeValue("chatMessagesArray");
        }

        [Export("insertObject:inChatMessagesArrayAtIndex:")]
        public void InsertMessage(ChatMessage message, nint index)
        {
            WillChangeValue("chatMessagesArray");
            _messages.Insert(message, index);
            DidChangeValue("chatMessagesArray");
        }

        [Export("removeObjectFromChatMessagesArrayAtIndex:")]
        public void RemoveMessage(nint index)
        {
            WillChangeValue("chatMessagesArray");
            _messages.RemoveObject(index);
            DidChangeValue("chatMessagesArray");
        }

        [Export("setChatMessagesArray:")]
        public void SetMessage(NSMutableArray array)
        {
            WillChangeValue("chatMessagesArray");
            _messages = array;
            DidChangeValue("chatMessagesArray");
        }
        #endregion
    }
}

