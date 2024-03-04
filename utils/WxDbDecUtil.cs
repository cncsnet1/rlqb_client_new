using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXDBDecrypt.NET;

namespace rlqb_client.utils
{
    /**
     * 微信数据解密
     */
    internal class WxDbDecUtil
    {

        /**
         *  
         * 
         */
        public static List<string> decDb( List<string> srcDb,string outDbName,string key)
        {

            List<string> dbs = new List<string>();
            try
            {
                
                foreach (var microMsg in srcDb)
                {
                    WxDecrypt.DecryptDB(microMsg, microMsg+outDbName ,key);
                    dbs.Add(microMsg + outDbName);
                }
                dbs.Sort();
                return dbs;
            }catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return dbs;
            }
            
        }
    }
}
