using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    /**
     * 
     * 登录之后的配置类
     */
    public class LoginConfig
    {
        //派出所名称
        public string govName {  get; set; }

        //组织架构xpath
        public string orgxpath { get; set; }

        /**
         * 是否开启ocr
         */
        public int openOcr {  get; set; }

        /**
         *  同音词处理
         */
        public int homophonic {  get; set; }

        /**
         *  pc的循环时间
         */

        public int pcRoundTime { get; set; }

        /**
         *  仅入库
         */
        public string roomFlag {  get; set; }

        /**
         *  词库
         */
        public List<string> words { get; set; }

        /**
         *  我的名称
         */
        public string name {  get; set; }




    }
}
