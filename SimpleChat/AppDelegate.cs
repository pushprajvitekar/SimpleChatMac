using AppKit;
using Foundation;

namespace SimpleChat
{
	[Register ("AppDelegate")]
	public class AppDelegate : NSApplicationDelegate
	{
		public AppDelegate ()
		{
		}
        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
			return true;
            //return base.ApplicationShouldTerminateAfterLastWindowClosed(sender);
        }
        public override void DidFinishLaunching (NSNotification notification)
		{
			// Insert code here to initialize your application
		}

		public override void WillTerminate (NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}
}

