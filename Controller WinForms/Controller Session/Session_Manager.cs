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
    enum ESessionType : int
    {
        FileManager = 0
    }

    struct SSession
    {
        public ESessionType type;
        public IControllerSession session;
    }

    class CSession_Manager
    {
        private List<SSession> Sessions { get; }

        private CConnection Connection { get; }

        private Random RandomInstance { get; }

        public CSession_Manager(CConnection Connection)
        {
            RandomInstance = new Random();
            Sessions = new List<SSession>();
            this.Connection = Connection;
        }

        int GenerateSessionId()
        {
            int Id = -1;

            while(Id == -1)
            {
                int attempt = RandomInstance.Next();

                bool success = true;

                foreach(SSession Session in Sessions)
                {
                    if(Session.session.Id == attempt)
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

        public void StartSession<T>(ESessionType type, SClient Client)
        {
            SSession Session = new SSession
            {
                type = type,
                session = (IControllerSession)Activator.CreateInstance<T>()
            };

            lock(Sessions)
            {
                Session.session.Id = GenerateSessionId();
                Session.session.Client = Client;
                Session.session.Connection = Connection;
                Session.session.StartSession();

                Sessions.Add(Session);
            }
        }

        public void SessionPacketReceived(SSessionPacket SessionPacket)
        {
            lock(Sessions)
            {
                foreach(SSession Session in Sessions)
                {
                    if(Session.session.Id == SessionPacket.SessionId)
                    {
                        Session.session.OnPacketReceived(SessionPacket.Packet, SessionPacket.data);
                        break;
                    }
                }
            }
        }
    }
}
