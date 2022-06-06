using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace LottoCalculator
{
    public partial class Form1 : Form
    {
        LottoCal lc;
        public Form1()
        {
            InitializeComponent();
            lc = new LottoCal();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Text = lc.WinningRate(6, 0);

            var one = lc.DrawWithoutRecord();
            button1.Text = one.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int cnt = 0;

            StreamReader sr = new StreamReader(@"E:\user\Desktop\樂透歷年開獎.csv");
            while(!sr.EndOfStream)
            {
                var str = sr.ReadLine();
                var infos = str.Split(',');

                LottoData ld = new LottoData();
                ld.Index = infos[0];
                ld.Date = infos[1];
                ld.Star1 = Convert.ToByte(infos[2]);
                ld.Star2 = Convert.ToByte(infos[3]);
                ld.Star3 = Convert.ToByte(infos[4]);
                ld.Star4 = Convert.ToByte(infos[5]);
                ld.Star5 = Convert.ToByte(infos[6]);
                ld.Star6 = Convert.ToByte(infos[7]);
                ld.Special = Convert.ToByte(infos[8]);

                lc.AddRecordData(ld);
                cnt++;
            }
            sr.Close();

            button2.Text = cnt.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = lc.Contain(5, 15, 26, 27, 34, 44).ToString();            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int cnt = 0;
            bool ok = false;
            do
            {
                var one = lc.Draw();
                ok = lc.Contain(one);
                cnt++;
            }
            while (!ok);

            button4.Text = cnt.ToString();
        }
    }
}
