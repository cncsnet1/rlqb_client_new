using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    internal class Wxmsg
    {
        public string name {  get; set; }
        public string wxid { get; set; }
        public string phone { get; set; }
        public string key { get; set; }
        public string version { get; set; }
        public List<string> msgDbs { get; set; }
        public List<string> microMsgDbs { get; set; }

    }
}
