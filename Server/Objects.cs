using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public enum PacketTypeClient
    {
        GetOpenKeyServer = 0x00,
        SetOpenKeyClient = 0x01,
        SetNickname = 0x02,
        GetNickname = 0x03,
        GetId = 0x04,
        GetRooms = 0x05,
        GetMyRoom = 0x06,
        CreateRoom = 0x07,
        JoinRoom = 0x08,
        SendMessage = 0x09,
        Error = 0xFE,
        OK = 0xFF
    }
    public enum PacketTypeServer
    {
        SetOpenKeyServer = 0x00,
        GetOpenKeyClient = 0x01,
        SetNickname = 0x03,
        GetNickname = 0x04,
        SetId = 0x05,
        SetRooms = 0x06,
        SetMyRoom = 0x07,
        SendMessage = 0x08,
        Error = 0xFE,
        OK = 0xFF
    }
    public class Room
    {
        public ushort Id { get; private set; }
        public string Name { get; set; }
        public Client Owner;
        public List<Client> Clients = new List<Client>();
        public bool Private { get; set; }

        public int openN;
        public int openE;
        private int closeF;
        private int closeE;

        public int openNClients;
        public int openEClients;

        public Room(Server server, Client owner)
        {
            Id = Utils.getUniqueId(server.Rooms);
            Owner = owner;
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
        public Socket socket;
        public Thread thread;
        public Room room;
        public Server server;


        private MemoryStream outputStream = new MemoryStream();
        private BinaryWriter outputWriter;
        private MemoryStream inputStream = new MemoryStream(new byte[2048], 0, 2048, true, true);
        private BinaryReader inputReader;

        public Client(Socket socket, Server server)
        {
            Id = Utils.getUniqueId(server.Clients);
            this.socket = socket;
            Initiate();
        }

        public void Initiate()
        {
            outputWriter = new BinaryWriter(outputStream);
            inputReader = new BinaryReader(inputStream);
            thread = new Thread(HandleClient);
            thread.Start();
        }


        public void HandleClient()
        {

            while (socket.Connected)
            {
                socket.Receive(inputStream.GetBuffer());
                PacketTypeClient packetCode = (PacketTypeClient)inputReader.ReadInt32();
                workPacketType(packetCode);
            }
        }

        private void send()
        {
            outputStream.Position = 0;
            socket.Send(outputStream.GetBuffer());
            outputStream.Flush();
        }

        private void workPacketType(PacketTypeClient packetType)
        {
            switch (packetType)
            {
                case PacketTypeClient.GetOpenKeyServer:
                    {

                        break;
                    }
                case PacketTypeClient.SetOpenKeyClient:
                    {
                        break;
                    }
                case PacketTypeClient.SetNickname:
                    {
                        int length = inputReader.ReadInt32();
                        string nickname = Encoding.UTF8.GetString(inputReader.ReadBytes(length));
                        outputWriter.Write((int)PacketTypeServer.OK);
                        Log.Info($"Client {Nickname} change nickname to {nickname}");
                        Nickname = nickname;
                        send();
                        break;
                    }
                case PacketTypeClient.GetNickname:
                    {
                        outputWriter.Write((int)PacketTypeServer.SetNickname);
                        byte[] nickname = Encoding.UTF8.GetBytes(Nickname);
                        outputWriter.Write(nickname.Length);
                        outputWriter.Write(nickname);
                        send();
                        break;
                    }
                case PacketTypeClient.GetId:
                    {
                        outputWriter.Write((int)PacketTypeServer.SetId);
                        outputWriter.Write(Id);
                        send();
                        break;
                    }
                case PacketTypeClient.GetRooms:
                    {
                        outputWriter.Write((int)PacketTypeServer.SetRooms);
                        outputWriter.Write(server.Rooms.Count);
                        foreach (Room room in server.Rooms)
                        {
                            outputWriter.Write(room.Id);
                            outputWriter.Write(room.Name);
                            outputWriter.Write(room.Private);
                        }
                        send();
                        break;
                    }
                case PacketTypeClient.GetMyRoom:
                    {
                        outputWriter.Write((int)PacketTypeServer.SetMyRoom);
                        outputWriter.Write(room.Id);
                        outputWriter.Write(room.Name);
                        outputWriter.Write(room.Private);
                        send();
                        break;
                    }
                case PacketTypeClient.CreateRoom:
                    {
                        if (room != null || Nickname.Replace(" ", "").Length == 0)
                        {
                            outputWriter.Write((int)PacketTypeServer.Error);
                            if (room != null) outputWriter.Write(1); // комната уже есть
                            else if (Nickname.Replace(" ", "").Length == 0) outputWriter.Write(2); // нет никнейма
                            else outputWriter.Write(0); // неизвестная ошибка
                        }
                        else
                        {
                            bool Private = inputReader.ReadBoolean();
                            room = server.createRoom(this);
                            room.Private = Private;
                            outputWriter.Write((int)PacketTypeServer.SetMyRoom);
                            outputWriter.Write(room.Id);
                            outputWriter.Write(room.Name);
                            outputWriter.Write(room.Private);
                            send();
                        }
                        send();
                        break;
                    }
                case PacketTypeClient.JoinRoom:
                    {
                        if (room != null || Nickname.Replace(" ", "").Length == 0)
                        {
                            outputWriter.Write((int)PacketTypeServer.Error);
                            if (room != null) outputWriter.Write(1); // комната уже есть
                            else if (Nickname.Replace(" ", "").Length == 0) outputWriter.Write(2); // нет никнейма
                            else outputWriter.Write(0); // неизвестная ошибка
                        }
                        else
                        {

                        }
                        break;
                    }
                case PacketTypeClient.SendMessage:
                    {
                        if (room == null)
                        {
                            outputWriter.Write((int)PacketTypeServer.Error);
                            outputWriter.Write(3); // не в комнате
                            send();
                            break;
                        }
                        break;
                    }
            }
        }
    }
}
