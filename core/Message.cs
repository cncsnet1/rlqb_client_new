using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    public class Message
    {

      
            public string LocalId { get; set; }
            public string TalkerId { get; set; }
            public string MsgSvrID { get; set; }
            public int Type { get; set; }
            public int SubType { get; set; }
            public int  IsSender { get; set; }
            public long CreateTime { get; set; }
            public int Sequence { get; set; }
            public int StatusEx { get; set; }
            public int FlagEx { get; set; }
            public int Status { get; set; }
            public int MsgServerSeq { get; set; }
            public int MsgSequence { get; set; }
            public string StrTalker { get; set; }
            public string StrContent { get; set; }
            public string DisplayContent { get; set; }
            public string Reserved0 { get; set; }
            public string Reserved1 { get; set; }
            public string Reserved2 { get; set; }
            public string Reserved3 { get; set; }
            public string Reserved4 { get; set; }
            public string Reserved5 { get; set; }
            public string Reserved6 { get; set; }
            public byte[] CompressContent { get; set; }
            public byte[] BytesExtra { get; set; }
            public byte[] BytesTrans { get; set; }
        

    }
}
