using System;
using AppKit;

namespace SimpleChat
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            try
            {


                NSApplication.Init();
                NSApplication.Main(args);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
