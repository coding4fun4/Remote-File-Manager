using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Controller_WinForms.Connection;
using SharedCode.Packets.Controller;
using SharedCode.Packets.Sessions;

namespace Controller_WinForms.Controller_Session
{
    class CSession_Manager
    {
        private List<IControllerSession> Sessions { get; }

        private CConnection Connection { get; }

        private Random RandomInstance { get; }

        public CSession_Manager(CConnection Connection)
        {
            RandomInstance = new Random();
            Sessions = new List<IControllerSession>();
            this.Connection = Connection;
        }

        int GenerateSessionId()
        {
            int Id = -1;

            while(Id == -1)
            {
                int attempt = RandomInstance.Next();

                bool success = true;

                foreach(IControllerSession Session in Sessions)
                {
                    if(Session.Id == attempt)
                    {
                        success = false;
                        break;
                    }
                }

                if(success)
                {
                    Id = attempt;
                }
            }

            return Id;
        }

        void SendIntroduction(IControllerSession session)
        {
            SSessionPacket SessionPacket = new SSessionPacket
            {
                SessionId = session.Id,
                ClientHandle = session.Client.SocketHandle,
                Packet = ESessionPackets.Introduction,
                ControllerKey = Program.Key
            };

            Connection.SendPacket<SSessionPacket>(EControllerPackets.Session, SessionPacket);
        }

        public void StartSession<T>(SClient Client)
        {
            IControllerSession session = (IControllerSession)Activator.CreateInstance<T>();

            session.Client = Client;
            session.Connection = Connection;

            lock (Sessions)
            {
                session.Id = GenerateSessionId();

                Sessions.Add(session);
            }

            SendIntroduction(session);
        }

        public void SessionPacketReceived(SSessionPacket SessionPacket)
        {
            lock(Sessions)
            {
                foreach(IControllerSession Session in Sessions)
                {
                    if(Session.Id == SessionPacket.SessionId)
                    {
                        Session.OnPacketReceived(SessionPacket.Packet, SessionPacket.data);
                        break;
                    }
                }
            }
        }
    }
}
