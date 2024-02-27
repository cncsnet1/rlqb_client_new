using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.core
{
    internal class ConfigUtil
    {
           [DllImport("kernel32")]
         private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);
         [DllImport("kernel32")]
         private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        //ini文件名称
         private static string inifilename = "config\\Config.ini";
         //获取ini文件路径
         private static string inifilepath = Directory.GetCurrentDirectory() + "\\" + inifilename;
 
         public static string GetValue(string key) { 
             StringBuilder s = new StringBuilder(1024);
             GetPrivateProfileString("conf_wx", key,"", s,1024, inifilepath);
             return s.ToString();
         }
 
 
         public static void SetValue(string key, string value)
         {
             try
             {
                WritePrivateProfileString("conf_wx", key, value, inifilepath);
             }
             catch (Exception ex)
            {
                throw ex;
          }
         }

        public static ClientConfig GetConfig()
        {
            ClientConfig config = new ClientConfig();
            config.host=GetValue("host");
            config.port=GetValue("port");
            config.onlineWords=GetValue("online_words");
            config.onlyGroup=GetValue("only_group");
            return config;
            
        }

    }
}
