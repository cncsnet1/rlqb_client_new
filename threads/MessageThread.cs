using Newtonsoft.Json;
using rlqb_client.core;
using rlqb_client.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WeChatGetKey;

/**
 * 消息线程
 */
namespace rlqb_client.threads
{
    internal class MessageThread
    {

        public void threat()
        {
            while (true)
            {

                List<string> words = Form1.loginConfig.words;
                int openOcr = Form1.loginConfig.openOcr;
                int pcRoundTime = Form1.loginConfig.pcRoundTime;
                int homophonic= Form1.loginConfig.homophonic;
                try
                {
                    List<Wxmsg> msgs = WxdumpUtil.GetWxmsgs();
                   
                    foreach (Wxmsg msg in msgs)
                    {
                       //首先解密两个db
                       List<string> decMsgDbPath=  WxDbDecUtil.decDb(msg.msgDbs, "_msg.db", msg.key);
                       List<string> decMirDbPath = WxDbDecUtil.decDb(msg.microMsgDbs, "_mir3.db", msg.key);
                       int messageLine = 0;
                       if(decMirDbPath.Count==0||decMsgDbPath.Count==0)
                        {
                            LogUtils.error(msg.name + "（" + msg.wxid + "）提取数据失败");
                            continue;
                        }
                       long progressIndex = 0;
                       if(File.Exists("config\\progress.json"))
                       {
                            LogUtils.info("存在进度文件，正在读取上次进展");
                            progressIndex= ProgressUtil.getInfo(msg.wxid);
                            if(progressIndex>0)
                            {
                                LogUtils.info(msg.name + "（" + msg.wxid + "）" + " 将从上次进度继续开始，进度时间为：" + progressIndex);
                            }
                            else
                            {
                                LogUtils.info(msg.name + "（" + msg.wxid + "）" + " 无进度，需要重新开始");

                            }
                        }
                        else
                        {
                            LogUtils.info(msg.name + "（" + msg.wxid + "）" + " 无进度，需要重新开始");

                        }

                        //开始循环消息db开始查询数据
                        foreach (string msgDbPath in decMsgDbPath)
                        {
                          List<Message> messages=  SqlUtil.getMessageByCreatetTime(msgDbPath, progressIndex);
                           foreach(Message message in messages)
                            {
                               MessageToServer messageToServer = new MessageToServer();
                                BeanUtil.CopyProperties(message, messageToServer);
                                messageToServer.msg_type = "群消息";
                                messageToServer.ChatRoomName = message.StrTalker;
                                //获取发送者的Id
                                string wxId=WechatParseUtil.getSendUserWxId(message.BytesExtra);


                                User sendUser= SqlUtil.getUserByUserName(decMirDbPath[0],wxId);
                                
                                if(sendUser!=null)
                                {
                                  
                                    

                                }
                                else
                                {

                                }
                                progressIndex = message.CreateTime;
                                messageLine++;
                            }
                        }


                        ProgressUtil.addOrEdit(msg.wxid, progressIndex);
                        LogUtils.info(msg.name + "（" + msg.wxid + "）" + "此次同步群消息 "+messageLine+" 条");
                    }
                }
                catch (Exception e)
                {
                    LogUtils.error("消息监听失败，请联系管理员:" + e.Message+"\t");
                }

                Thread.Sleep(1000 * pcRoundTime);
            }

        }





    }
}
