using System;
using AppKit;

namespace SimpleChat.Models
{
	
    public class ChatMessageDelegate : NSTableViewDelegate
    {
        #region Constants 
        private const string CellIdentifier = "ProdCell";
        #endregion

        #region Private Variables
        private ChatDataSource DataSource;
        #endregion

        #region Constructors
        public ChatMessageDelegate(ChatDataSource datasource)
        {
            this.DataSource = datasource;
        }
        #endregion

        #region Override Methods
        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = CellIdentifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            // Setup view based on the column selected
            switch (tableColumn.Title)
            {
                case "Date":
                    view.StringValue = DataSource.Messages[(int)row].EntryDate;
                    break;
                case "Sender":
                    view.StringValue = DataSource.Messages[(int)row].Sender;
                    break;
                case "Message":
                    view.StringValue = DataSource.Messages[(int)row].Message;
                    break;
            }

            return view;
        }
        #endregion
    }
}

