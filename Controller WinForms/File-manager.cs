using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller_WinForms.Connection;
using Controller_WinForms.Controller_Session;
using SharedCode.Packets.Controller;
using SharedCode.Packets.Sessions;

namespace Controller_WinForms
{
    public partial class FrmFileManager : Form, IControllerSession
    {
        public CConnection Connection { get; set; }

        public int Id { get; set; }

        public SClient Client { get; set; }

        public FrmFileManager()
        {
            InitializeComponent();
        }

        private void FrmFileManager_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("File Manager [{0}@{1}:{2}]", Client.UserName, Client.IPAddress, Client.SocketHandle);

            SendIntroduction();
        }

        void SendIntroduction()
        {
           
        }

        public void OnPacketReceived(ESessionPackets packet, byte[] arguments)
        {
            
        }

        public void StartSession()
        {
            this.Show();
        }
    }
}
