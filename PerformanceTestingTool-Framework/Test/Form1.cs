using PerformanceTestingTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PerformanceTest p = new PerformanceTest();
            p.SetCount(10);//循环次数(默认:1)
            p.SetIsMultithread(true);//是否启动多线程测试 (默认:false)
            p.Execute(AAA, new string[] { "第一次的参数", "第二次的参数" }, BBB);
        }


        public void AAA(string parameter)
        {
            Thread.Sleep(1000);
        }

        public void BBB(string parameter)
        {
            MessageBox.Show(this, parameter);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PerformanceTest p = new PerformanceTest();
            p.SetCount(10);//循环次数(默认:1)
            p.SetIsMultithread(false);//是否启动多线程测试 (默认:false)
            p.Execute(AAA, new string[] { "第一次的参数", "第二次的参数" }, BBB);
        }
    }
}
