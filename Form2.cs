using rlqb_client.core;
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
            string host= ConfigUtil.GetValue("host");
            string port=ConfigUtil.GetValue("port");
            this.textBox1.Text = host;
            this.textBox2.Text = port;
            string online_workds= ConfigUtil.GetValue("online_words");
            string only_group= ConfigUtil.GetValue("only_group");

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
            MessageBox.Show("保存成功");
        }
    }
}
