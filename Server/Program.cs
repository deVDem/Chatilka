using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        
        public static List<Server> Servers = new List<Server>();

        static void Main(string[] args)
        {
            Console.Title = "Chatilka Server";
            Log.Info("Program started. Starting servers..");
            for (int i = 0; i < 1; i++)
            {
                Servers.Add(new Server(228+i, 10, 10, 1));
                Thread.Sleep(10);
            }
            while (true)
            {
                AcceptCommand(Console.ReadLine());
            }
        }

        static void AcceptCommand(string command)
        {

        }
    }
}
