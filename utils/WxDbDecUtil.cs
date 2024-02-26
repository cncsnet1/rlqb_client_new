﻿using System;
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

                return dbs;
            }catch (Exception e)
            {
                return dbs;
            }
            
        }
    }
}