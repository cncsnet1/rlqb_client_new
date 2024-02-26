using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    public class ChatRoom 
    {
        public int RowId { get; set; }
        public string ChatRoomName { get; set; }
        public string UserNameList { get; set; }
        public string NickName { get; set; }
        public string Remark { get; set; }

        public List<string> UserNameLists { get; set; }

       
    }

}
