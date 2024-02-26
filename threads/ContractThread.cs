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

    internal class ContractThread
    {

        private string dbName;
        private int secTime;
        private bool isComplete;

        public ContractThread(int secTime,bool isComplete) {
            this.secTime = secTime;
            this.isComplete = isComplete;
            
        }


        public void threat()
        {
           while (true)
            {
                try
                {
                    List<Wxmsg> msgs = WxdumpUtil.GetWxmsgs();
                    foreach (Wxmsg msg in msgs)
                    {
                        List<string> microDbs= msg.microMsgDbs;
                        string outDbName = "";
                        if(this.isComplete)
                        {
                            outDbName = "_mir1.db";
                        }
                        else
                        {
                            outDbName = "_mir2.db";

                        }
                        List<string> decPath= WxDbDecUtil.decDb(microDbs,outDbName, msg.key);
                        if(decPath.Count == 0)
                        {
                            continue;
                        }
                        string microdb = decPath[0];
                          List<ChatRoom> chatrooms= SqlUtil.getChatRooms(microdb);
                            foreach (var item in chatrooms)
                            {
                                Console.WriteLine(item.NickName);
                            Console.WriteLine(item.UserNameLists.Count + "");
                            }
                            chatrooms=null;
                   
                        


                    }
                }
                catch (Exception e)
                { 
                    Console.WriteLine(e.ToString());
                }
                
                Thread.Sleep(1000*secTime);
            }

        }
    }
}
