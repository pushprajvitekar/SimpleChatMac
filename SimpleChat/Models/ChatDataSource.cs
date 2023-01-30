using System;
using AppKit;
using StoreKit;
using System.Collections.Generic;

namespace SimpleChat.Models
{
	
    public class ChatDataSource : NSTableViewDataSource
    {
        #region Public Variables
        public List<ChatMessage> Messages = new List<ChatMessage>();
        #endregion

        #region Constructors
        public ChatDataSource()
        {
        }
        #endregion

        #region Override Methods
        public override nint GetRowCount(NSTableView tableView)
        {
            return Messages.Count;
        }
        #endregion
    }
}

