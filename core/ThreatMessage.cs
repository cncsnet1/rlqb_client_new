using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    internal class ThreatMessage
    {
        public int localId { get; set; }
        public int TalkerId { get; set; }
        public int MsgSvrID { get; set; }
        public int Type { get; set; }
        public int SubType { get; set; }
        public int IsSender { get; set; }
        public string CreateTime { get; set; }
        public long Sequence { get; set; }
        public int StatusEx { get; set; }
        public int FlagEx { get; set; }
        public int Status { get; set; }
        public int MsgServerSeq { get; set; }
        public long MsgSequence { get; set; }
        public string StrTalker { get; set; }
        public string StrContent { get; set; }
        public string DisplayContent { get; set; }
        public int Reserved0 { get; set; }
        public int Reserved1 { get; set; }
        public int NAVICAT_ROWID { get; set; }
        public string msg_type { get; set; }
        public string TalkWindow { get; set; }
        public string WeChatAccount { get; set; }
        public string msg_sender { get; set; }
        public string msg_receiver { get; set; }
        public long intCreateTime { get; set; }
        public string org { get; set; }

        public string xpath {  get; set; }
        public string ver {  get; set; }

        public string my_doc_id {  get; set; }

        public int last_Heartbeat {  get; set; }
    }
}
