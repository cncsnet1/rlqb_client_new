using QueryEngine;
using rlqb_client.core;
using rlqb_client.threads;
using rlqb_client.utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChatGetKey;
using WXDBDecrypt.NET;

namespace rlqb_client
{
    public partial class Form1 : Form
    {
        private List<FileAndDirectoryEntry> entries = new List<FileAndDirectoryEntry>();
        public static List<Wxmsg> onlineAccount = new List<Wxmsg>();
        public static LoginConfig loginConfig;
        public static ClientConfig clientConfig;

        public Form1()
        {
            //日志对象

           
            // 设置 Console.Write 输出到自定义的 InterceptingWriter
           
            InitializeComponent();
            //初始化日志对象
            LogUtils.init(listBox3);
            //初始化微信的内存数据
            blockUpdate();
            clientConfig = ConfigUtil.GetConfig();
            // LoadEntriesAsync();
        }

        private void blockUpdate()
        {
           
            HostsUtil.AddOrEdit("127.0.0.1\tdldir1.qq.com");
            HostsUtil.AddOrEdit("127.0.0.1\tszminorshort.weixin.qq.com");
            HostsUtil.AddOrEdit("127.0.0.1\tszextshort.weixin.qq.com");
         

        }

        private void LoadEntriesAsync()
        {
           
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    this.button1.Text = "初始化中，请稍等...";
                    this.entries = Engine.GetAllFilesAndDirectories();
                    this.button1.Invoke(new Action(() =>
                    {
                        this.button1.Text = "读取配置并开始监测";
                        this.button1.Enabled = true;
                    }));
                    //SearchResultAsync("msg0");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

                  
                }
            });
        }

        private void SearchResultAsync(string FilterString)
        {
            Task.Factory.StartNew(() => {
                Thread.Sleep(500);
                string filterString = FilterString;

                var filteredResult = this.entries
                    .Where(f => f.FileName != null && f.FileName.IndexOf(filterString, StringComparison.OrdinalIgnoreCase) > -1)
                    .OrderBy(f => f.FileName)
                    .ToList();

                if (FilterString != filterString)
                {
                    return;
                }

                foreach (var entry in filteredResult)
                {
                  
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string username= textBox2.Text;
            string password = textBox3.Text;
            LoginConfig loginConfigResult= HttpUtils.loginGetConfig(username, password);
            
            if (loginConfigResult != null)
            {
                /**
                 * 优先处理界面问题
                 */
                this.listBox1.Items.Clear();
                foreach(var word in loginConfigResult.words)
                {
                    this.listBox1.Items.Add(word);
                }
                //用户配置信息查看
                this.textBox4.Text = "\r\n\r\n" + "用户信息：" + loginConfigResult.govName + "-" + loginConfigResult.name + "\r\n\r\n" +
                    "监测频率（秒每次）：" + loginConfigResult.pcRoundTime + "\r\n\r\n" +
                    "群聊过滤规则：" + (loginConfigResult.roomFlag==null||"".Equals(loginConfigResult.roomFlag)?"全部群聊":(loginConfigResult.roomFlag)) ;
                this.button1.Enabled = false;

                //初始化消息工具类
                
                loginConfig=loginConfigResult;
                MessageUtil.init();
               

                //要开始工作，跑线程了
                initOnlineWx();
             
                Thread t = new Thread(new TheatThread().threat);
                Thread c1 = new Thread(new ContractThread(60 * 60 * 12, true).threat);
                Thread c2 = new Thread(new ContractThread(60 * 2 , false).threat);
                Thread m1 = new Thread(new MessageThread().threat);
                t.Start();
                c1.Start();
                c2.Start();
                m1.Start();
            }

        }

        /**
         * 初始化/更新在线微信情况
         */
        private void initOnlineWx()
        {
            this.listBox2.Invoke(new Action(() => {
                this.listBox2.Items.Clear();
            }));
            
            onlineAccount= WxdumpUtil.GetWxmsgs();
            foreach (var data in onlineAccount)
            {
                this.Invoke(new Action(() => {
                    this.listBox2.Items.Add(data.name + "（" + data.wxid + "）");

                }));
                
            }

        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); 
            form2.ShowDialog();
        }
    }


}
