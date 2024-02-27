using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.utils
{

    /**
     * 维护进度文件
     */
    internal class ProgressUtil
    {

        /**
         * 
         *  添加或修改
         */
        public static void addOrEdit(string wxId,long date)
        {

            using (FileStream fs = new FileStream("config\\progress.json", FileMode.OpenOrCreate))
            {
                JObject groJson = null;
                int len = (int) fs.Length;
                byte[] buf=new byte[len];
                fs.Read(buf, 0, len); 
                string str=System.Text.Encoding.UTF8.GetString(buf);
               
                if (!str.Trim().Equals(""))
                {
                    groJson =JObject.Parse(str);
                    groJson[wxId] = date;
                }
                else
                {
                    groJson = new JObject();
                    if (date != 0)
                    {
                        groJson[wxId] = date;

                    }
                    else
                    {
                        groJson[wxId] = 0;

                    }
                }
          
                fs.Seek(0, SeekOrigin.Begin);
                byte[] newBuf= Encoding.Default.GetBytes(groJson.ToString());
                fs.Write(newBuf, 0, newBuf.Length);
                fs.Close();
            }

        }

        public static long getInfo(string wxId)
        {
            using (FileStream fs = new FileStream("config\\progress.json", FileMode.OpenOrCreate))
            {
                JObject groJson = null;

                int len = (int)fs.Length;
                byte[] buf = new byte[len];
                fs.Read(buf, 0, len);
                string str = System.Text.Encoding.UTF8.GetString(buf);

                if (!str.Trim().Equals(""))
                {
                    groJson = JObject.Parse(str);
                    return groJson[wxId].Value<long>();
                }
                else
                {
                    groJson = new JObject();
                    return 0;
                }
                fs.Close();
            }

        }

    }
}
