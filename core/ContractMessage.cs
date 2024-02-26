using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    public class ContractMessage
    {
        public string UserName { get; set; }
        public string RoomDisplayName { get; set; }
        public string ChatRoomName { get; set; }
        public string RoomRemark { get; set; }
        public string UserDisplayName { get; set; }
        public string UserRemark { get; set; }
        public string bigHeadImgUrl { get; set; }
        public string msg_type { get; set; }
        public string wx_id { get; set; }
        public string TalkWindow { get; set; }
        public string msg_sender { get; set; }
        public string my_doc_id { get; set; }
        public string org { get; set; }
        public string xpath { get; set; }
        public string ver { get; set; }
        public string WeChatAccount { get; set; }
        public string CreateTime { get; set; }
    }
}
