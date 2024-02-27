using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rlqb_client.utils
{
    internal class WechatParseUtil
    {

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
    }
}
