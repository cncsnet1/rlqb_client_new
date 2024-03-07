using Pinyin4net.Format;
using Pinyin4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Policy;
using LZ4;
using rlqb_client.core;
using System.Runtime.InteropServices;

namespace rlqb_client.utils
{
    internal class WechatParseUtil
    {

        [DllImport("liblz4.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LZ4_decompress_safe(byte[] source, byte[] dest, int compressedSize, int maxDecompressedSize);


        /**
         * 
         *  获取发信人的Id
         */
        public static string getSendUserWxId(byte[] ex)
        {
            if (ex == null) return "";
            string resultString = Encoding.GetEncoding("iso-8859-1").GetString(ex);

            string pattern = @"[a-zA-Z_0-9\-]{5,30}";

            // 使用正则表达式匹配模式
            Match match = Regex.Match(resultString, pattern);

            // 检查是否匹配成功并输出结果
            if (match.Success)
            {
                string result = match.Value;
                return result;
            }
            return "";
        }

        /**
         *  获取拼音
         */
        public static string GetPinyin(string chineseCharacter)
        {
            HanyuPinyinOutputFormat format = new HanyuPinyinOutputFormat();
            format.ToneType=(HanyuPinyinToneType.WITHOUT_TONE);
            string data = "";
            foreach(char  ch in chineseCharacter)
            {
                string[] pinyinArray = PinyinHelper.ToHanyuPinyinStringArray(ch, format);

                if (pinyinArray != null && pinyinArray.Length > 0)
                {
                    // 取第一个拼音 

                    data += pinyinArray[0] + " ";
                }
            }
            // 转换为拼音数组
            

            return data;
        }

        private static string RemoveInvalidXmlChars(string text)
        {
            return new string(text.Where(c => XmlConvert.IsXmlChar(c)).ToArray());
        }

        public static String getWechatContent(MessageToServer message)
        {

            try
            {
                if (message.Type == 34)
                {
                    return getVoiceContent(message.StrContent);
                }
                else if (message.Type == 48)
                {
                    return getMapContent(message.StrContent);
                }
                else if (message.Type == 47)
                {
                    return getEmojContent(message.StrContent);
                }
                else if (message.Type == 43)
                {
                    return "【人力情报视频消息识别：待完善】";
                }
                else if (message.Type == 3)
                {
                    return "【人力情报图片消息识别：待完善】";
                }
                else if (message.Type == 49)
                {
                    byte[] compressedData = message.CompressContent;

                    // 假设 row 是包含压缩数据的 DataRow 或类似对象
                 

                    // 假设您知道未压缩数据的大小（替换为实际值）
                    int uncompressedSize = 0x10004;

                    // 创建用于存储解压缩后数据的字节数组
                    byte[] decompressedData = new byte[uncompressedSize];

                    // 使用 LZ4 原生 C 函数进行解压缩
                    int result = LZ4_decompress_safe(compressedData, decompressedData, compressedData.Length, uncompressedSize);

                    if (result< 0)
                    {
                        return message.StrContent;
                    }

                        // 解压缩成功
                    string resultString = System.Text.Encoding.UTF8.GetString(decompressedData);
                    resultString =resultString.Substring(0, resultString.IndexOf("</msg>") + 6);
                    if(message.SubType==6)
                    {
                       string fileName= getChatFileName(resultString);
                       if(Form1.loginConfig.openOcr==1)
                        {
                            
                        }
                        else
                        {
                            return "【人力情报文件】" + fileName;
                        }
                    }else if(message.SubType==7)
                    {

                    }




                }
                return message.StrContent;
            }catch (Exception e)
            {
                return message.StrContent;
            }
        }

        /**
         * 
         *  获取语音信息
         */
        private static string getVoiceContent(string content)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            XmlNode xml = xmlDocument.SelectSingleNode("/msg/voicetrans");
            
            if (xml == null) return content;
            if (xml.Attributes["transtext"] == null) return "【人力情报语音消息转换】未转换成功";
            string transtext= xml.Attributes["transtext"].Value;
            return "【人力情报语音消息转换】" + transtext;
            
        }
        /**
         *  获取地图数据
         */
        private static string getMapContent(string content)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            XmlNode location = xmlDocument.SelectSingleNode("/msg/location");
            if (location == null) return content;
            return "【人力情报定位转换】" + " 经度：" + location.Attributes["x"] + " 维度：" + location.Attributes[
                 "y"] + " 定位名称：" + location.Attributes[
                 "label"] + "-" + location.Attributes["poiname"];
        }

        /**
         *  获取表情包
         */
        private static string getEmojContent(string content)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            XmlNode location = xmlDocument.SelectSingleNode("/msg/emoji");
            if (location == null) return content;
            if (location.Attributes["cdnurl"] == null) return "【人力情报表情包转换】 系统表情包";
            string url = location.Attributes["cdnurl"].Value;
            url = url.Replace("&amp;", "");
            return "【人力情报表情包转换】" + url;

        }
        private static string getChatFileName(string content)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content.Substring(0,content.Length));
            XmlNode titleNode= xmlDocument.SelectSingleNode("/msg/appmsg/title");
            if (titleNode == null) return content;
            
            return titleNode.InnerText;
        }
    }
}
