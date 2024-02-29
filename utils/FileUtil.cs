using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace rlqb_client.utils
{
    /**
     *  文件操作类，白名单 黑名单 本地关键词
     */
    internal class FileUtil
    {
        public static string readFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
              
                int len = (int)fs.Length;
                byte[] buf = new byte[len];
                fs.Read(buf, 0, len);
                string str = System.Text.Encoding.UTF8.GetString(buf);
                fs.Close();
                return str;
            }
        }

        public static void writeFile(string path, string content)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                
                int len = (int)fs.Length;
               
                byte[] newBuf = Encoding.UTF8.GetBytes(content);
                fs.Write(newBuf, 0, newBuf.Length);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
