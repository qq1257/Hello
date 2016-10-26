using XMPP.Tags.Jabber.Client;

namespace PressureHeight.Im
{
    public interface MsgListener
    {
         void processMsg(Message message);
    }
}
