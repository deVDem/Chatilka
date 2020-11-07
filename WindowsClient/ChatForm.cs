using System.Windows.Forms;

namespace Client
{
    public partial class ChatForm : Form
    {
        public delegate void addMsgToTextBox(string msg);
        public addMsgToTextBox myDelegate;
        public ChatForm()
        {
            InitializeComponent();
        }
    }
}
