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
using MyLottery;
using MyLottery.Ticket;
using MyLottery.Computer;

namespace LottoCalculator
{
    public partial class Form1 : Form
    {
        LottoComputer lc;
        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lsvHistory.Items.Clear();
            lsvTicket.Items.Clear();

            lc = new LottoComputer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var one = lc.DrawWithoutRecord();
            button1.Text = one.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<CsvItem> csvItems = new List<CsvItem>();
            csvItems.Add(CsvItem.Index);
            csvItems.Add(CsvItem.Date);
            csvItems.Add(CsvItem.Star1);
            csvItems.Add(CsvItem.Star2);
            csvItems.Add(CsvItem.Star3);
            csvItems.Add(CsvItem.Star4);
            csvItems.Add(CsvItem.Star5);
            csvItems.Add(CsvItem.Star6);
            csvItems.Add(CsvItem.Star7);
            lc.LoadHistoryCSV(@"E:\user\Desktop\樂透歷年開獎.csv", csvItems);

            var history = lc.History;

            lsvHistory.BeginUpdate();
            for (int i = 0; i < history.Count; i++)
            {
                var info = history[i];
                lsvHistory.Items.Add(info.Index);
                lsvHistory.Items[i].SubItems.Add(info.Date);
                lsvHistory.Items[i].SubItems.Add(info.BasicStars[0].ToString());
                lsvHistory.Items[i].SubItems.Add(info.BasicStars[1].ToString());
                lsvHistory.Items[i].SubItems.Add(info.BasicStars[2].ToString());
                lsvHistory.Items[i].SubItems.Add(info.BasicStars[3].ToString());
                lsvHistory.Items[i].SubItems.Add(info.BasicStars[4].ToString());
                lsvHistory.Items[i].SubItems.Add(info.BasicStars[5].ToString());
                lsvHistory.Items[i].SubItems.Add(info.Special.ToString());
            }
            lsvHistory.EndUpdate();

            button2.Text = lc.NumberOfHistory.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<byte> stars = new List<byte>();
            stars.Add(2);
            stars.Add(14);
            stars.Add(16);
            stars.Add(22);
            stars.Add(46);
            stars.Add(37);
            button3.Text = lc.Contain(stars).ToString();            
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

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Text = LottoComputer.WinningCount(1, 0).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Text = LottoComputer.WinningRate().ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button7.Text = LottoComputer.Expectation(0).ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LottoTicket lottoTicket = new LottoTicket(true);
            var Awards = lc.MatchHistory(lottoTicket).Where(r => r != MyLottery.Award.None);
            button8.Text = Awards.Count().ToString();

            Console.Clear();
            foreach(var award in Awards)
            {
                Console.WriteLine(award);
            }
        }    
    }
}
