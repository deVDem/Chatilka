using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        public ushort Id { get; private set; }
        public ushort maxRooms { get; private set; }
        public ushort maxUsers { get; private set; }
        public ushort listenCount { get; private set; }

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private IPEndPoint endPoint;

        private Random random = new Random();
        public List<Room> Rooms { get; private set; }
        public List<Client> Clients { get; private set; }


        public int numberClients()
        {
            return Clients.Count;
        }

        public int numberRooms()
        {
            return Rooms.Count;
        }

        public Server(int port, ushort maxRooms, ushort maxUsers, ushort listenCount)
        {
            Rooms = new List<Room>();
            Clients = new List<Client>();
            endPoint = new IPEndPoint(IPAddress.Any, port);
            Id = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
            Log.Info($"Starting server with id {Id}..");
            this.maxRooms = maxRooms;
            this.maxUsers = maxUsers;
            this.listenCount = listenCount;
            socket.Bind(endPoint);
            socket.Listen(listenCount);
            Log.Info($"Server started on port {port}");
            socket.BeginAccept(AcceptConnection, null);
        }

        private void AcceptConnection(IAsyncResult ar)
        {
            Socket connectionSocket = socket.EndAccept(ar);
            string ipAddressClient = IPAddress.Parse(((IPEndPoint)connectionSocket.RemoteEndPoint).Address.ToString()).ToString();
            Log.Info($"Подключение клиента {ipAddressClient} ..");
            Client client = new Client(connectionSocket);
            socket.BeginAccept(AcceptConnection, null);
        }
    }
}
