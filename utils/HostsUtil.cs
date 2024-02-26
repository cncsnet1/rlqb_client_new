using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.utils
{
    public static class HostsUtil
    {
        public static string GetHostFileFullName()
        {
            string abc = Environment.GetEnvironmentVariable("systemroot");
            string fileName = Path.Combine(abc, @"System32\drivers\etc\hosts");

            return fileName;
        }

        /// <summary>
        /// 域名是否存在
        /// </summary>
        /// <param name="hostsLineValue">格式：api.yun.comn</param>
        /// <returns></returns>
        public static bool IsDomainEx(string inputValue)
        {
            string fileName = GetHostFileFullName();

            //兼容 IP+空格+域名
            string domain = inputValue;
            if (!string.IsNullOrEmpty(inputValue) && inputValue.Trim().Contains(" "))
            {
                var hostLineArray = inputValue.Trim().Split(' ');
                if (hostLineArray.Length > 1)
                {
                    domain = hostLineArray[1];
                }
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    while (sr.Peek() >= 0)
                    {
                        string curLine = sr.ReadLine();
                        if (!string.IsNullOrEmpty(curLine) && !curLine.Trim().StartsWith("#"))
                        {
                            var hostLineArray = curLine.Trim().Split(' ');
                            if (hostLineArray.Length > 1)
                            {
                                string lineDomain = hostLineArray[1];
                                if (!string.IsNullOrEmpty(lineDomain) && lineDomain.Trim() == domain)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// 删除域名绑定
        /// </summary>
        /// <param name="hostsLineValue">格式：api.yun.comn</param>
        /// <returns></returns>
        public static bool Del(string inputValue)
        {
            string fileName = GetHostFileFullName();

            if (!IsDomainEx(inputValue))
            {
                //不存在返回，视为成功。
                return true;
            }

            //兼容 IP+空格+域名
            string domain = inputValue;
            if (!string.IsNullOrEmpty(inputValue) && inputValue.Trim().Contains(" "))
            {
                var hostLineArray = inputValue.Trim().Split(' ');
                if (hostLineArray.Length > 1)
                {
                    domain = hostLineArray[1];
                }
            }

            StringBuilder scHosts = new StringBuilder();
            //修改
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    while (sr.Peek() >= 0)
                    {
                        string curLine = sr.ReadLine();
                        //只处理非#号开头的，微软的原注释保留
                        if (!string.IsNullOrEmpty(curLine) && !curLine.Trim().StartsWith("#"))
                        {
                            var hostLineArray = curLine.Trim().Split(' ');
                            if (hostLineArray.Length > 1)
                            {
                                string lineDomain = hostLineArray[1];
                                if (!string.IsNullOrEmpty(lineDomain) && lineDomain.Trim() == domain)
                                {
                                    //如果是要删除的那行，直接跳过
                                    continue;
                                }
                            }
                        }

                        scHosts.AppendLine(curLine);
                    }
                }
            }

            //使hosts文件可写
            FileInfo ff = new FileInfo(fileName);
            if (ff.Attributes != FileAttributes.Normal)
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
            }

            string hostsContent = scHosts.ToString();
            //写
            using (FileStream fs = new FileStream(fileName, FileMode.Truncate, FileAccess.Write))
            {
                using (StreamWriter sr = new StreamWriter(fs, Encoding.UTF8))
                {
                    sr.Write(hostsContent);
                }
            }
            return true;
        }


        /// <summary>
        /// 添加或修改域名绑定
        /// </summary>
        /// <param name="hostsLineValue">格式：192.168.0.33 api.yun.comn</param>
        /// <returns></returns>
        public static bool AddOrEdit(string inputValue)
        {
            inputValue = inputValue.Trim();//去除前后空格


            string fileName = GetHostFileFullName();

            if (IsDomainEx(inputValue))
            {
                //存在则先删除旧的。新旧要绑定的IP可能会不一样。
                Del(inputValue);
            }

            //使hosts文件可写
            FileInfo ff = new FileInfo(fileName);
            if (ff.Attributes != FileAttributes.Normal)
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
            }

            bool writeEmptyLine = false;
            string orgCon = GetHostsContent();
            if (!orgCon.EndsWith("\r\n"))
            {
                writeEmptyLine = true;
            }

            //修改
            using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sr = new StreamWriter(fs, Encoding.UTF8))
                {
                    if (writeEmptyLine)
                    {
                        //非换行结尾的，添加上换行。
                        //避免出现  “192.168.0.7 cn.bing.co5192.168.0.8 cn.bing.co6”
                        sr.Write("\r\n");
                    }
                    sr.WriteLine(inputValue);
                }
            }

            return true;
        }

        public static string GetHostsContent()
        {
            string con = string.Empty;
            string fileName = GetHostFileFullName();
            if (!File.Exists(fileName))
            {
                return con;
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    con = sr.ReadToEnd();
                }
            }

            return con;
        }

    }
}
