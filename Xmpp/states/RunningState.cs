// Copyright © 2006 - 2012 Dieter Lunn
// Modified 2012 Paul Freund ( freund.paul@lvl3.org )
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
// You should have received a copy of the GNU Lesser General Public License along
// with this library; if not, write to the Free Software Foundation, Inc., 59
// Temple Place, Suite 330, Boston, MA 02111-1307 USA

using XMPP.Tags;
using XMPP.Common;
using XMPP.Tags.Jabber.Client;

namespace XMPP.States
{
    public class RunningState : IState
    {
        public RunningState(Manager manager) : base(manager)
        {
            Manager.Events.OnSend += OnSend;
        }

        public override void Dispose()
        {
            Manager.Events.OnSend -= OnSend;
        }

        private void OnSend(object sender, TagEventArgs e)
        {
            Manager.Connection.Send(e.tag);
        }

        public override void Execute(Tag data = null)
        {
            if (data != null)
            {
                if (data is Iq)
                {
                    Iq iq = data as Iq;
                    if (iq.Payload != null)
                    {
                        if (iq.Payload is Tags.Xmpp.Ping.Ping)
                        {
                            Iq newiq = new Iq { IdAttr = iq.IdAttr, TypeAttr = Iq.TypeEnum.result };
                            Manager.Connection.Send(newiq);
                        }
                    }
                }
                Manager.Events.Receive(this, data);
            }               
        }
    }
}