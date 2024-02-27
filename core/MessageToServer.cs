using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    public class MessageToServer : Message
    {
        public string wx_id { get; set; }
        public string msg_type { get; set; }

        public string ChatRoomName { get; set; }

        public string TalkWindow { get; set; }
        public string WeChatAccount { get; set; }

        public string msg_sender { get; set; }

        public string msg_receiver { get; set; }

        public int intCreateTime { get; set; }

        public string cloud_word { get; set; }

        public string area_word { get; set; }

        public string org { get; set; }

        public string xpath { get; set; }

        public string my_doc_id { get; set; }

        public string ver { get; set; } 




    }
}
