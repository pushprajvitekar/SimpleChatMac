// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SimpleChat
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton btnConnect { get; set; }

		[Outlet]
		AppKit.NSButton btnSend { get; set; }

		[Outlet]
		AppKit.NSTableView tblViewChatMessages { get; set; }

		[Outlet]
		AppKit.NSTableView tblViewFriends { get; set; }

		[Outlet]
		AppKit.NSTableView tblViewLogs { get; set; }

		[Outlet]
		AppKit.NSTextField txtFriendIpAddress { get; set; }

		[Outlet]
		AppKit.NSTextField txtFriendPort { get; set; }

		[Outlet]
		AppKit.NSTextField txtMessage { get; set; }

		[Outlet]
		AppKit.NSTextField txtMyIpAddress { get; set; }

		[Outlet]
		AppKit.NSTextField txtMyPort { get; set; }

		[Action ("btnConnect_Click:")]
		partial void btnConnect_Click (Foundation.NSObject sender);

		[Action ("btnSend_Click:")]
		partial void btnSend_Click (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (btnConnect != null) {
				btnConnect.Dispose ();
				btnConnect = null;
			}

			if (btnSend != null) {
				btnSend.Dispose ();
				btnSend = null;
			}

			if (txtMessage != null) {
				txtMessage.Dispose ();
				txtMessage = null;
			}

			if (tblViewChatMessages != null) {
				tblViewChatMessages.Dispose ();
				tblViewChatMessages = null;
			}

			if (tblViewFriends != null) {
				tblViewFriends.Dispose ();
				tblViewFriends = null;
			}

			if (tblViewLogs != null) {
				tblViewLogs.Dispose ();
				tblViewLogs = null;
			}

			if (txtFriendIpAddress != null) {
				txtFriendIpAddress.Dispose ();
				txtFriendIpAddress = null;
			}

			if (txtFriendPort != null) {
				txtFriendPort.Dispose ();
				txtFriendPort = null;
			}

			if (txtMyIpAddress != null) {
				txtMyIpAddress.Dispose ();
				txtMyIpAddress = null;
			}

			if (txtMyPort != null) {
				txtMyPort.Dispose ();
				txtMyPort = null;
			}
		}
	}
}
