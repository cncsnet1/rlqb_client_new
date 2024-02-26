using Serilog.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using Serilog.Events;

namespace rlqb_client.utils
{
    internal class LogUtils
    {
        private static ILogger logger;
      
      

        public static void init(ListBox outBox)
        {
            logger = new LoggerConfiguration().WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day).WriteTo.Sink(new ListBoxSink(outBox)).CreateLogger();
            
        }
       

        public static void info(string message)
        {
            logger.Information(message); 
           
        }
        public static void error(string message)
        {
            logger.Error(message);
        }
    }
    class ListBoxSink : ILogEventSink
    {

        private readonly ListBox listBox;

        public ListBoxSink(ListBox listBox)
        {
            this.listBox = listBox ?? throw new ArgumentNullException(nameof(listBox));
        }
        public void Emit(LogEvent logEvent)
        {
            // 将日志信息添加到 ListBox 中
            listBox.Invoke((Action)(() =>
            {
                if(listBox.Items.Count >=1000)
                {
                    listBox.Items.RemoveAt(0);
                }
                string formattedLog = $"{logEvent.Timestamp:yyyy-MM-dd HH:mm:ss} [{logEvent.Level}] - {logEvent.RenderMessage()}";

                listBox.Items.Add(formattedLog);
                listBox.SelectedIndex = listBox.Items.Count - 1;
            }));
        }
    }
}
