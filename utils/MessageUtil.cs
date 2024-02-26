using rlqb_client.core;
using Serilog;
using Serilog.Sinks.Network;
using Serilog.Sinks.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlqb_client.utils
{
    internal class MessageUtil
    {
        private static ILogger logger;

        public static void init()
        {
            ClientConfig config=  ConfigUtil.GetConfig();
            logger = new LoggerConfiguration()
           .WriteTo.Sink(new ReconnectingNetworkSink(config.host,config.port))
           .CreateLogger();
        }
        public static void sendMessage(string msg)
        {
            logger.Error(msg);
        }
        

    }
}
