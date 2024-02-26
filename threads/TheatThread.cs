using Newtonsoft.Json;
using rlqb_client.core;
using rlqb_client.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WeChatGetKey;

namespace rlqb_client.threads
{
    /**
     * 心跳线程
     */

    internal class TheatThread
    {
        public void threat()
        {
           while (true)
            {
                try
                {
                    List<Wxmsg> msgs = WxdumpUtil.GetWxmsgs();
                    foreach (Wxmsg msg in msgs)
                    {
                        ThreatMessage threatMessage = new ThreatMessage();
                        threatMessage.localId = 0;
                        threatMessage.TalkerId = 1;
                        threatMessage.MsgSvrID = 0;
                        threatMessage.Type = 1;
                        threatMessage.SubType = 0;
                        threatMessage.IsSender = 0;

                        threatMessage.Sequence = 1680154655000;
                        threatMessage.StatusEx = 0;
                        threatMessage.FlagEx = 0;
                        threatMessage.Status = 2;
                        threatMessage.MsgServerSeq = 1;
                        threatMessage.MsgSequence = 789060180;
                        threatMessage.StrTalker = "L Y";
                        threatMessage.StrContent = "";
                        threatMessage.DisplayContent = "";
                        threatMessage.Reserved0 = 0;
                        threatMessage.Reserved1 = 2;
                        threatMessage.NAVICAT_ROWID = 1217;
                        threatMessage.msg_type = "Heartbeat";
                        threatMessage.TalkWindow = "";
                        threatMessage.msg_sender = "L Y";
                        threatMessage.msg_receiver = "Vinson";
                        threatMessage.intCreateTime = 1680154655;
                        threatMessage.org = Form1.loginConfig.govName;
                        threatMessage.xpath = Form1.loginConfig.orgxpath;
                        threatMessage.ver = Form1.version;
                        threatMessage.WeChatAccount = msg.name + "(" + msg.wxid + ")";
                        threatMessage.CreateTime = TimeUtil.getNowDateStr();
                        threatMessage.last_Heartbeat = (int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        threatMessage.my_doc_id = EncUtil.md5enCry(threatMessage.last_Heartbeat + "");
                        string dataJson = JsonConvert.SerializeObject(threatMessage);
                        string data = EncUtil.sendMessageEnc(dataJson);
                        MessageUtil.sendMessage(data);
                        data = null;
                        dataJson= null;
                        threatMessage = null;
                        msgs = null;
                    }
                }
                catch (Exception e)
                { 
                    LogUtils.error("心跳维持失败，请联系管理员:" + e.Message);
                }
                
                Thread.Sleep(1000*60);
            }

        }
    }
}
