using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMPP.Tags;

namespace PressureHeight.Im
{
    /// <summary>
    /// 数据包监听
    /// </summary>
   public interface PacketListener
    {
        void processPacket(Tag packet);
    }
}
