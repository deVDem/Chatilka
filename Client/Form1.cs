using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public delegate void addMsgToTextBox(string msg);
        public addMsgToTextBox myDelegate;
        public Form1()
        {
            InitializeComponent();
            myDelegate = new addMsgToTextBox(addMsgToTextBoxMethod);
            form = this;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (socket.Connected)
            {
                MemoryStream ms = new MemoryStream(new byte[1024], 0, 1024, true, true);
                BinaryWriter writer = new BinaryWriter(ms);
                byte[] bytesNick = Encoding.UTF8.GetBytes(nickTextBox.Text);
                byte[] bytesMsg = Encoding.UTF8.GetBytes(messageTextBox.Text);
                writer.Write(bytesNick.Length);
                writer.Write(bytesNick);
                writer.Write(bytesMsg.Length);
                writer.Write(bytesMsg);
                socket.Send(ms.GetBuffer());
                messageTextBox.Text = "";
            }
        }
        private Form1 form;
        private void connectButton_Click(object sender, EventArgs e)
        {
            if (!socket.Connected)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipTextBox.Text), Convert.ToInt32(textBox1.Text));
                socket.Connect(endPoint);
                connectButton.Enabled = false;
                sendButton.Enabled = true;
                Thread thread = new Thread(HandleServer);
                thread.Start(socket);
            } else
            {
                connectButton.Enabled = false;
                sendButton.Enabled = true;
            }
        }
        
        void addMsgToTextBoxMethod(string msg)
        {
            msgsTextBox.Text += Environment.NewLine;
            msgsTextBox.Text += msg;
            
        }

        private void HandleServer(object o)
        {
            MemoryStream ms = new MemoryStream(new byte[1024], 0, 1024, true, true);
            BinaryReader reader = new BinaryReader(ms);
            Socket server = (Socket)o;
            while(true)
            {
                server.Receive(ms.GetBuffer());
                int lengthName = reader.ReadInt32();
                string name = Encoding.UTF8.GetString(reader.ReadBytes(lengthName));
                int lengthMsg = reader.ReadInt32();
                string msg = Encoding.UTF8.GetString(reader.ReadBytes(lengthMsg));
                ms.Position = 0;
                form.Invoke(myDelegate,name + "> " + msg);
            }
        }
    }
}
