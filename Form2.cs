using rlqb_client.core;
using rlqb_client.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rlqb_client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            ClientConfig clientConfig= Form1.clientConfig;
            this.textBox1.Text = clientConfig.host;
            this.textBox2.Text = clientConfig.port;
            string online_workds= clientConfig.onlineWords;
            string only_group= clientConfig.onlyGroup;

            string filter_check = ConfigUtil.GetValue("filter_check");


            if ("0".Equals(filter_check))
            {
                radioButton7.Checked = true;
                
            }
            else if ("1".Equals(filter_check))
            {
                radioButton3.Checked = true;
            }
            else
            {
                radioButton6.Checked = true;
            }

            if ("0".Equals(online_workds) )
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            if ("0".Equals(only_group))
            {
                radioButton5.Checked = true;
                radioButton4.Checked = false;
            }
            else
            {
                radioButton4.Checked = true;
                radioButton5.Checked = false;
            }
            
            //本地关键词
           string gjc= FileUtil.readFile("config\\关键词.txt");
            Console.WriteLine(gjc);
            textBox5.Text= gjc;

            string whiteFile = FileUtil.readFile("config\\白名单.txt");
            textBox4.Text= whiteFile;

            string blackFile = FileUtil.readFile("config\\黑名单.txt");
            textBox3.Text =blackFile;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigUtil.SetValue("host", textBox1.Text);
            ConfigUtil.SetValue("port", textBox2.Text);
            if(radioButton1.Checked )
            {
                ConfigUtil.SetValue("online_words", "1");
            }else if(radioButton2.Checked )
            {
                ConfigUtil.SetValue("online_words", "0");
            }

            if (radioButton4.Checked)
            {
                ConfigUtil.SetValue("only_group", "1");
            }
            else if (radioButton5.Checked)
            {
                ConfigUtil.SetValue("only_group", "0");
            }

            if (radioButton7.Checked)
            {
                ConfigUtil.SetValue("filter_check", "0");
            }
            else if (radioButton3.Checked)
            {
                ConfigUtil.SetValue("filter_check", "1");
            }
            else if (radioButton6.Checked)
            {
                ConfigUtil.SetValue("filter_check", "2");
            }

           
            FileUtil.writeFile("config\\关键词.txt", textBox5.Text);
            FileUtil.writeFile("config\\白名单.txt", textBox4.Text);
            FileUtil.writeFile("config\\黑名单.txt", textBox3.Text);

            Form1.clientConfig = ConfigUtil.GetConfig();

            MessageBox.Show("保存成功");
        }
    }
}
