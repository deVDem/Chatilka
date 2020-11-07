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
        public static ushort getUniqueId()
        {
            // TODO: сделать уникальность
            Random random = new Random();
            int id = random.Next();
            return (ushort)id;
        }

        static void Main(string[] args)
        {
            Console.Title = "Chatilka Server";
            socket.BeginAccept(AcceptUser, null);
            while (true)
            {
                AcceptCommand(Console.ReadLine());
            }
        }

        private static void AcceptUser(IAsyncResult ar)
        {
            Socket userSocket = socket.EndAccept(ar);
            Thread thread = new Thread(HandleUser);
            thread.Start(userSocket);
            socket.BeginAccept(AcceptUser, null);
        }
        private static List<Socket> userSockets = new List<Socket>();

        private static void SendAll(byte[] send)
        {
            foreach (Socket item in userSockets)
            {
                item.Send(send);
            }
        }

        static void HandleUser(object obj)
        {
            Socket user = (Socket)obj;
            userSockets.Add(user);
            MemoryStream ms = new MemoryStream(new byte[1024], 0, 1024, true, true);
            BinaryReader reader = new BinaryReader(ms);

            while(true)
            {
                user.Receive(ms.GetBuffer());
                int lengthName = reader.ReadInt32();
                string name = Encoding.UTF8.GetString(reader.ReadBytes(lengthName));
                int lengthMsg = reader.ReadInt32();
                string msg = Encoding.UTF8.GetString(reader.ReadBytes(lengthMsg));
                ms.Position = 0;
                SendAll(ms.GetBuffer());
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(name);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("> " + msg);
            }
        }
        

        static void AcceptCommand(string command)
        {

        }
    }
}
