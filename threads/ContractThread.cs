using Newtonsoft.Json;
using rlqb_client.core;
using rlqb_client.utils;
using Serilog.Core;
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

      
        private int secTime;
        private bool isComplete;

        /**
         * 这样同步过的微信_群_联系人的数据在增量的时候就不会重复同步了
         * 只有全量的时候才会同步，而且全量的时候也会自动补充list中没有的
         * 但是也要考虑list的容量遍变大，最大值范围等资源问题
         */
        private static List<string> contractMd5 = new List<string>();

        public ContractThread(int secTime,bool isComplete) {
            this.secTime = secTime;
            this.isComplete = isComplete;
            
        }


        public void threat()
        {
           
           if(!isComplete)
            {
                Thread.Sleep(1000*60*5);
            }

           while (true)
            {
                try
                {
                    
                    //过去当前微信内存情况，原则上支持多开
                    List<Wxmsg> msgs = WxdumpUtil.GetWxmsgs();
                    //获取验证方式
                    string fileterCheck= Form1.clientConfig.fileterCheck;
                    foreach (Wxmsg msg in msgs)
                    {
                        int userSum = 0;
                        int groupSum = 0;
                        
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
                        //开始循环- 群+用户 唯一。
                        foreach (ChatRoom chatroom in chatrooms)
                        {
                            if("1".Equals(fileterCheck))
                            {
                                string[] whiteNames=Form1.clientConfig.whiteName;
                                bool nickNameCheck = whiteNames.Any(wn => chatroom.NickName.IndexOf(wn) != -1);
                                bool remarkCheck = whiteNames.Any(wn => chatroom.Remark.IndexOf(wn) != -1);

                                if (!nickNameCheck && !remarkCheck) continue;
                            }
                            else if ("2".Equals(fileterCheck))
                            {
                                string[]blackName = Form1.clientConfig.blackName;
                                bool nickNameCheck = blackName.Any(wn => chatroom.NickName.IndexOf(wn) != -1);
                                bool remarkCheck = blackName.Any(wn => chatroom.Remark.IndexOf(wn) != -1);
                                if(nickNameCheck||remarkCheck) continue;
                            }
                        Console.WriteLine(chatroom.NickName + " " + chatroom.Remark);
                           List<string>chatUsers= chatroom.UserNameLists;
                           foreach(string cu in chatUsers)
                            {
                                    string md5Str = msg.wxid+"_"+ chatroom.ChatRoomName + "_" + cu;
                                    string md5=EncUtil.md5enCry(md5Str);
                                    md5Str = null;
                                if (contractMd5.Contains(md5))
                                    {
                                        if(!isComplete)
                                        {
                                            continue;
                                         }
                                    }
                                    else
                                    {
                                        contractMd5.Add(md5);
                                    }
                                
                               

                               userSum++;
                               ContractMessage contractMessage= new ContractMessage();
                               contractMessage.ChatRoomName = chatroom.ChatRoomName;
                               contractMessage.UserName = cu;
                               contractMessage.RoomDisplayName = chatroom.NickName + "（" + chatroom.ChatRoomName+"）";
                               contractMessage.TalkWindow = chatroom.NickName + "（" + chatroom.ChatRoomName + "）";


                               contractMessage.RoomRemark = chatroom.Remark;
                               contractMessage.msg_type = "wx_contact";
                               contractMessage.wx_id = cu;
                                //开始循环发送赋值
                                User user=  SqlUtil.getUserByUserName(microdb, cu);
                                if (user != null)
                                {
                                    contractMessage.UserDisplayName = user.NickName + "（" + user.UserName + "）";
                                    contractMessage.msg_sender = user.NickName + "（" + user.UserName + "）";
                                    contractMessage.UserRemark = user.Remark;
                                    if (user.BigHeadImgUrl != null && user.BigHeadImgUrl.Length > 5)
                                    {
                                       contractMessage.bigHeadImgUrl=user.BigHeadImgUrl;
                                    }
                                    else
                                    {
                                        contractMessage.bigHeadImgUrl = "#";
                                    }
                                }
                                else
                                {
                                    contractMessage.msg_sender = "no_msg_sender";
                             
                                    contractMessage.bigHeadImgUrl = "#";
                                }
                                contractMessage.CreateTime=TimeUtil.getNowDateStr();
                                contractMessage.ver = Form1.version;
                                contractMessage.xpath = Form1.loginConfig.orgxpath;
                                contractMessage.org = Form1.loginConfig.govName;    
                                contractMessage.WeChatAccount = msg.name + "（" + msg.wxid + "）";
                                contractMessage.my_doc_id = chatroom.ChatRoomName.Replace("@", "-") + "-" + cu;
                                string dataJson = JsonConvert.SerializeObject(contractMessage);
                                string data = EncUtil.sendMessageEnc(dataJson);
                                MessageUtil.sendMessage(data);
                                contractMessage = null;
                                
                            }
                            groupSum++;
                        }
                        LogUtils.info("["+(isComplete?"全量":"增量") +"] "+msg.name + "（" + msg.wxid + "）此次同步了" + groupSum + "个群, "+userSum+" 个成员");

                    }
                }
                catch (Exception e)
                {
                    LogUtils.error("[" + (isComplete ? "全量" : "增量") + "] 成员同步失败："+e.Message);
                }
                
                Thread.Sleep(1000*secTime);
            }

        }
    }
}
