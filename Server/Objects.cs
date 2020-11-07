using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Room
    {
        public ushort Id { get; private set; }
        public string Name { get; set; }
        public List<Client> Clients = new List<Client>();
        public bool Private { get; set; }

        public Room()
        {
            Random random = new Random();
            Id = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
            Name = "Room " + Id.ToString();
        }

        public int numberClients()
        {
            return Clients.Count;
        }
    }

    public class Client
    {
        public ushort Id { get; private set; }
        public string Nickname { get; set; }
        public Socket Socket;
        public Thread Thread;

        public Client(Socket socket)
        {
            Id = Program.getUniqueId();
            Socket = socket;
            
        }

        public void Initiate()
        {

        }
    }
}
