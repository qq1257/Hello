using DataBase;
using PressureHeight.Im;
using PressureHeight.models;
using PressureHeight.models.im;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using XMPP;
using XMPP.Tags;
using XMPP.Tags.Jabber.Client;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PressureHeight.view.im
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SessionPage : Page,PacketListener
    {
        private ObservableCollection<SessionModel> tsL = null;
        private ImManager iManager = null;
        //是否开启计时器
        private bool isTimeTag = false;
        private Timer timer = null;
        private Dictionary<string, SessionModel> cacheSession = new Dictionary<string, SessionModel>();
        public SessionPage()
        {
            this.InitializeComponent();
            ImDbManage db = ImDbManage.getInstance();
            //db.addSessionDate("1", "你好", TimeUtil.getNowStamp());
            //db.setNameImg("1","中国好网民","gif");

            //db.addSessionDate("2", "啊啊啊", TimeUtil.getNowStamp());
            //db.setNameImg("2", "威威威威", "gif");

            //db.addSessionDate("3", "孙大发", TimeUtil.getNowStamp());
            //db.setNameImg("3", "3333", "gif");

            //db.addSessionDate("4", "asdfasdf", TimeUtil.getNowStamp());
            //db.setNameImg("4", "是的风格十分的", "gif");


            var sessions = db.getSessions();
            tsL = new ObservableCollection<SessionModel>(sessions);
            //dm.closeDB();
            Session_L.ItemsSource = tsL;

            iManager = ImManager.getInstance();
            iManager.Login("test", "123456", "啦啦啦");
        }

        private void Session_L_ItemClick(object sender, ItemClickEventArgs e)
        {
            tsL[0].name = "大王叫我来巡山";
            tsL[0].timeStamp = TimeUtil.getNowStamp();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            iManager.addPacketListener(this);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            iManager.removePacketListener(this);
            base.OnNavigatingFrom(e);
        }

        public void processPacket(Tag packet)
        {
            if (packet is Message)
            {
                Message msg = packet as Message;
                Jid from = new Jid(msg.FromAttr);
                string body = msg.Body;

                if (cacheSession.ContainsKey(from.User))
                {
                    SessionModel t = cacheSession[from.User];
                    t.timeStamp = TimeUtil.getNowStamp();
                    t.newContent = body;
                    t.unreadNum++;
                }
                else
                {
                    cacheSession.Add(from.User, new SessionModel() { userId = from.User, timeStamp = TimeUtil.getNowStamp(), newContent = body, unreadNum = 1 });
                }


                if (!isTimeTag)
                {
                    TimerCallback timerc = new TimerCallback(timerTask);
                    timer = new Timer(timerc, null, 2000, 0);
                    isTimeTag = true;
                }
            }
            Debug.WriteLine("TEST:" + packet);
        }


        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="a"></param>
        private async void timerTask(object a)
        {
            Debug.WriteLine("11");
            await Dispatcher.RunIdleAsync((IdleDispatchedHandlerArgs e) =>
            {
                foreach (var s in cacheSession.Values)
                {
                    updateContent(s);                    
                }
                cacheSession.Clear();
            });
            isTimeTag = false;
            timer.Dispose();
        }

        private void updateContent(SessionModel s)
        {
            foreach (var t in tsL)
            {
                if (t.userId.Equals(s.userId))
                {
                    t.timeStamp = s.timeStamp;
                    t.newContent = s.newContent;
                    t.unreadNum += s.unreadNum;
                    return;
                }
            }
            tsL.Add(s);
        }

    }
}
