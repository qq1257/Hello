using XMPP;
using XMPP.Extensions;
using XMPP.Tags.Jabber.Client;

namespace PressureHeight.Im
{
    public class Chat
    {
        private Client client;
        private string to;
        private string loginJid;
        Message.TypeEnum type;
        public Chat(Client client, string to,string loginJid, Message.TypeEnum type)
        {
            this.client = client;
            this.to = to;
            this.loginJid = loginJid;
            this.type = type;
        }
        public void send(string body)
        {
            Message msg = new Message {TypeAttr=type,ToAttr=to,FromAttr =loginJid};
            var b = new Body {Value=body };
            msg.Add(b);
            client.SendAsync(msg);
        }
    }

}
