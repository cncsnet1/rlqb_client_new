using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    /**
     * 客户端配置文件配置类
     */
    internal class ClientConfig
    {
        //服务器地址
        public string host {  get; set; }

        //端口
        public string port {  get; set; }

        //网络关键词
        public string onlineWords {  get; set; }


        //只抓群消息

        public string onlyGroup { get; set;}
    }
}
