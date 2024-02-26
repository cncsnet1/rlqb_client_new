using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXDBDecrypt.NET;

namespace rlqb_client.utils
{
    internal class WxDbDecUtil
    {

        /**
         *  
         * 
         */
        public static bool decDb( List<string> srcDb,string outDbName,string key)
        {
            try
            {
                
                foreach (var microMsg in srcDb)
                {
                    WxDecrypt.DecryptDB(microMsg, microMsg, key);
                }
                
                return true;
            }catch (Exception e)
            {
                return false;
            }
            
        }
    }
}
