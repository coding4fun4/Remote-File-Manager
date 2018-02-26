using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharedCode.Packets.Controller;
using SharedCode.Packets.Sessions;

using Controller_WinForms.Connection;
using Controller_WinForms.Controller_Session;

namespace Controller_WinForms
{
    public partial class FrmController : Form
    {
        private CConnection Connection { get; }

        private CSession_Manager SessionManager { get; }

        public FrmController()
        {
            InitializeComponent();

            Connection = new CConnection(Program.Host, Program.Port);
            Connection.OnConnectionAuthorized += Connection_OnConnectionAuthorized;
            Connection.OnConnectionAuthorizationFailed += Connection_OnConnectionAuthorizationFailed;
            Connection.OnUpdateKey += Connection_OnUpdateKey;
            Connection.OnUpdateClient += Connection_OnUpdateClient;
            Connection.OnRemoveClient += Connection_OnRemoveClient;
            Connection.OnSessionPacketReceived += Connection_OnSessionPacketReceived;

            SessionManager = new CSession_Manager(Connection);
        }

        private void Connection_OnSessionPacketReceived(SSessionPacket SessionPacket)
        {
            SessionManager.SessionPacketReceived(SessionPacket);
        }

        void UpdateTitle(string title)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    this.Text = title;
                });
            }
            else this.Text = title;
        }

        private void Connection_OnUpdateKey(int key)
        {
            Program.Key = key;

            UpdateTitle(string.Format("Controller [WinForms] [{0}]", key));
        }

        void RemoveClient(int index)
        {
            for (int i = 0; i < LvClients.Items.Count; i++)
            {
                if (i == index)
                {
                    LvClients.Items[i].Remove();
                    break;
                }
            }
        }

        private void Connection_OnRemoveClient(int index)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    RemoveClient(index);
                });
            }
            else RemoveClient(index);
        }

        void SetClient(int index, SClient Client)
        {
            ListViewItem item;

            if (LvClients.Items.Count <= index)
            {
                item = new ListViewItem
                {
                    Text = Client.SocketHandle.ToString()
                };

                item.SubItems.Add(Client.IPAddress);
                item.SubItems.Add(Client.UserName);
                item.SubItems.Add(Client.OperatingSystem);
                item.Tag = (object)Client;

                LvClients.BeginUpdate();
                LvClients.Items.Add(item);
                LvClients.EndUpdate();

                return;
            }

            item = LvClients.Items[index];
            item.Text = Client.SocketHandle.ToString();
            item.SubItems[0].Text = Client.IPAddress;
            item.SubItems[1].Text = Client.UserName;
            item.SubItems[2].Text = Client.OperatingSystem;
            item.Tag = (object)Client;

            LvClients.BeginUpdate();
            LvClients.Items[index] = item;
            LvClients.EndUpdate();
        }

        private void Connection_OnUpdateClient(int index, SClient Client)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    SetClient(index, Client);
                });
            }
            else SetClient(index, Client);
        }

        void SetStatusText(string status)
        {
            LblStatus.Text = string.Format("Status: {0}", status);
        }

        private void Connection_OnConnectionAuthorizationFailed()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    SetStatusText("Password is invalid!");
                });
            }
            else SetStatusText("Password is invalid!");
        }

        private void Connection_OnConnectionAuthorized()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    SetStatusText("Authorized!");
                });
            }
            else SetStatusText("Authorized!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            Connection.StartConnectingToServer();
        }

        private void MiStartFileManagerSession_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in LvClients.SelectedItems)
            {
                SessionManager.StartSession<FrmFileManager>((SClient)item.Tag);
            }
        }
    }
}
