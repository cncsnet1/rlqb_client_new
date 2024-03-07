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

namespace rlqb_client.utils
{
    internal class WechatParseUtil
    {


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
                else if (message.Type == 49 && message.SubType==19)
                {
                    byte[] compressedData = message.CompressContent;

                    byte[] decompressedData = LZ4.LZ4Codec.Unwrap(compressedData, 0x10004);

                    // 去除末尾的一个字节
                    byte[] trimmedData = new byte[decompressedData.Length - 1];
                    Array.Copy(decompressedData, trimmedData, trimmedData.Length);

                    // 将字节数组解码为 UTF-8 字符串
                    string utf8String = Encoding.UTF8.GetString(trimmedData);


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
    }
}
