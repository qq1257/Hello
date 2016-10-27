using PressureHeight.config;
using PressureHeight.Im;
using System;
using System.Diagnostics;
using Windows.Storage;
using XMPP;
using XMPP.Tags;
using XMPP.Tags.Jabber.Client;

namespace PressureHeight
{
    /// <summary>
    ///IM通讯管理类 
    /// </summary>
    public class ImManager
    {
        private Client client;
        private static ImManager im;
        private Jid loginJid;
        private MsgListener chatMsgListener = null;
        private string chatTo;
        private ImManager()
        {

        }
        public static ImManager getInstance()
        {
            if (im == null)
                im = new ImManager();
            return im;
        }
        
        private object getLocalSettings(string name)
        {
           var settings=ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(name))
            {
                return settings.Values[name];
            } else
            {
                throw new Exception("No local configuration found:"+name);
            }
        }
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="resource">用户登陆源</param>
        public void Login(string user,string password,string resource)
        {
            try {
                client = new Client(Transport.Socket);
                client.Settings.Hostname = getLocalSettings(TotalStaticVariable.SERVICE_HOST).ToString();
                client.Settings.SSL = false;
                client.Settings.OldSSL = false;
                client.Settings.AuthenticationTypes = MechanismType.DigestMd5;
                client.Settings.Port = int.Parse(getLocalSettings(TotalStaticVariable.SERVICE_PORT).ToString());
                loginJid = new Jid(user, getLocalSettings(TotalStaticVariable.SERVICE_DOMIN).ToString(), resource);
                client.Settings.Id = loginJid;
                client.Settings.Password = password;
                client.OnConnected += Client_OnConnected;
                client.OnDisconnected += Client_OnDisconnected;
                client.OnError += Client_OnError;
                client.OnLogMessage += Client_OnLogMessage;
                client.OnNewTag += Client_OnNewTag;
                client.OnReady += Client_OnReady;
                client.OnReceive += Client_OnReceive;
                client.OnResourceBound += Client_OnResourceBound;
                client.Connect();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                client = null;
            }            
        }
        /// <summary>
        /// 退出连接
        /// </summary>
        public void disconnect()
        {
            if (client != null)
            {
                client.Disconnect();
                client = null;
             }
        }


        public Chat createChat(string to, MsgListener listener)
        {
            this.chatMsgListener = listener;
            chatTo = to;
            return new Chat(client, to, loginJid, Message.TypeEnum.chat);
        }

        public void removeChat()
        {
            chatMsgListener = null;
            chatTo = null;
        }

        private void Client_OnResourceBound(object sender, XMPP.Common.ResourceBoundEventArgs e)
        {
            Debug.WriteLine("Client_OnResourceBound:" + e.ToString());
            //throw new NotImplementedException();
        }
        //登陆成功后的消息监听
        private void Client_OnReceive(object sender, XMPP.Common.TagEventArgs e)
        {
            Tag tag = e.tag;
            if (tag is Message)
            {               
                if (chatMsgListener != null)
                {
                    Message msg = tag as Message;
                    if (msg.FromAttr.Contains(chatTo))
                    {
                        chatMsgListener.processMsg(msg);
                    }
                }
            }
            else if (tag is Iq)
            {

            }
            else if (tag is Presence)
            {

            }

            Debug.WriteLine("Client_OnReceive:" + e.tag);
            //throw new NotImplementedException();
        }

        private void Client_OnReady(object sender, EventArgs e)
        {
            Debug.WriteLine("Client_OnReady:" + e);
            //throw new NotImplementedException();
        }

        private void Client_OnNewTag(object sender, XMPP.Common.TagEventArgs e)
        {
            Debug.WriteLine("Client_OnNewTag:" + e.tag);
            //throw new NotImplementedException();
        }

        private void Client_OnLogMessage(object sender, XMPP.Common.LogEventArgs e)
        {
            Debug.WriteLine("Client_OnLogMessage:" + e.message.ToString());
            //throw new NotImplementedException();
        }

        private void Client_OnError(object sender, XMPP.Common.ErrorEventArgs e)
        {
            Debug.WriteLine("Client_OnError:" + e.message.ToString());
            //throw new NotImplementedException();
        }

        private void Client_OnDisconnected(object sender, EventArgs e)
        {
            Debug.WriteLine("Client_OnDisconnected:" + e);
            //throw new NotImplementedException();
        }

        private void Client_OnConnected(object sender, EventArgs e)
        {
            Debug.WriteLine("Client_OnConnected:" + e);
            //throw new NotImplementedException();
        }
    }
}
