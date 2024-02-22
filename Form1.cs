using QueryEngine;
using rlqb_client.core;
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
        List<Wxmsg> onlineAccount = new List<Wxmsg>();

        public Form1()
        {
           
            getConfig();
            InitializeComponent();
            initOnlineWx();
          

            // LoadEntriesAsync();
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
                    textBox1.Invoke(new Action(() => textBox1.Text +=entry.FullFileName+"\r\n"));
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchResultAsync("msg0");

        }

        private void initOnlineWx()
        {
            this.listBox2.Items.Clear();
            this.onlineAccount= Wxdump.ReadTest();
            foreach (var data in this.onlineAccount)
            {
                this.listBox2.Items.Add(data.name+"（"+data.wxid+"）");
                List<string> microMsgDbs= data.microMsgDbs;
                List<string> msgDbs = data.msgDbs;
                foreach (var microMsg in microMsgDbs)
                {
                    WxDecrypt.DecryptDB(microMsg, microMsg + "temp.db", data.key);
                }
                foreach(var msg in msgDbs)
                {
                    WxDecrypt.DecryptDB(msg, msg + "temp.db", data.key);

                }


            }

        }

        private void getConfig()
        {
            ConfigUtil.GetValue("host");
            ConfigUtil.GetValue("port");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); 
            form2.ShowDialog();
        }
    }


}
