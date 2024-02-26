using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.utils
{
    internal class TimeUtil
    {
        public static string getNowDateStr()
        {
            // 获取现在时间
            DateTime nowUtc = DateTime.UtcNow;

            // 设置中国时区
            TimeZoneInfo chinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            DateTime nowChina = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, chinaTimeZone);

            // 格式化日期时间字符串
            string formatStr = "yyyy-MM-ddTHH:mm:ss.fffZ";
            string chinaFormat = nowChina.ToString(formatStr);
            return chinaFormat;
        }
        public void main() { 
            Console.WriteLine(getNowDateStr());
        }
    }
}
