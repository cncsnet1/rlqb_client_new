using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using rlqb_client.utils;
using System.Net.Http;
using System.Security.Policy;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace rlqb_client.core
{
    internal class HttpUtils
    {
        public static string loginUrl = "https://{0}:10001/rlqb/get_user_conf/{1}/{2}/{3}";

        private static RestClientOptions getOption(string url)
        {
            var options = new RestClientOptions(url)
            {
                ConfigureMessageHandler = handler =>
                   new HttpClientHandler
                   {
                       ServerCertificateCustomValidationCallback = delegate { return true; }
                   }
            };
            return options;
        }

       

        public static LoginConfig loginGetConfig(string username,string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请填写用户名和密码");
                return null;
            }

            ClientConfig config= ConfigUtil.GetConfig();

            string enc_pwd = EncUtil.md5enCry(password);
            string signStr = string.Concat(Enumerable.Repeat(username +enc_pwd, 15));
            string mySign =EncUtil.md5enCry(signStr).Substring(0, 20);


            string loginUrlT = string.Format(loginUrl, config.host, username, enc_pwd,mySign);

            var option= getOption(loginUrlT);
            RestSharp.RestClient restClient = new RestSharp.RestClient(option);
            RestRequest request = new RestRequest(loginUrlT);
            RestResponse response = null;
            try
            {
                response = restClient.Get(request);
            }
            catch (Exception ex)
            {
                LogUtils.error("获取配置文件失败，请联系管理员：" + ex.Message);
           
                return null;

            }
            string content = response.Content;
            JObject result = JObject.Parse(content);
            int status = (int)result["status"];
            if(status == 0 )
            {
                string msg= (string) result["msg"];
                LogUtils.error("登录失败：" + msg);
                return null;
            }
            else
            {
                JObject data = (JObject)result["data"];
                LoginConfig loginConfig = new LoginConfig();
                loginConfig.govName = (string)data["gov_name"];
                loginConfig.homophonic = (int)data["homophonic"];
                loginConfig.pcRoundTime = (int)data["pc_round_time"];
                loginConfig.openOcr = (int)data["open_ocr"];
                loginConfig.orgxpath = (string)data["xpath"];
                loginConfig.roomFlag = (string)data["room_flag"];
                loginConfig.name = (string)data["name"];
                //判断是使用在线关键词库还是用这个
                if ("1".Equals(config.onlineWords))
                {
                    string wordsStr = (string)data["words"];
                    //这个位置记录一下要改的 套了两层words
                    JObject wordsObject = JObject.Parse(wordsStr);
                    List<string> words = wordsObject["words"].Select(item => item.ToObject<string>()).ToList();
                    loginConfig.words = words;
                }
                else
                {

                }
                LogUtils.info("登录成功");
                return loginConfig;
            }
        }


        public static void getKeyWord(string keyWord)
        {
            
        }
    }
}
