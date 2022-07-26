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
        SuperLottoComputer slc;
        public Form1()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lsvHistory.Items.Clear();
            lsvTicket.Items.Clear();

            lc = new LottoComputer();
            slc = new SuperLottoComputer();
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

            if (cbbGame.SelectedIndex > 0)
            {
                slc.LoadHistoryCSV(@"威力彩歷年開獎.csv", csvItems);
                var history = slc.History;

                lsvHistory.BeginUpdate();
                lsvHistory.Items.Clear();
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
                    lsvHistory.Items[i].SubItems.Add(info.SecendStar.ToString());
                }
                lsvHistory.EndUpdate();

                button2.Text = slc.NumberOfHistory.ToString();
            }
            else
            {
                lc.LoadHistoryCSV(@"大樂透歷年開獎.csv", csvItems);
                var history = lc.History;

                lsvHistory.BeginUpdate();
                lsvHistory.Items.Clear();
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
            if (cbbGame.SelectedIndex > 0)
                button6.Text = SuperLottoComputer.WinningRate().ToString();
            else
                button6.Text = LottoComputer.WinningRate().ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (cbbGame.SelectedIndex > 0)
                button7.Text = SuperLottoComputer.Expectation(200000000).ToString();
            else
                button7.Text = LottoComputer.Expectation(100000000).ToString();
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

        private void button9_Click(object sender, EventArgs e)
        {
            int i = 0;
            bool win, winR;
            bool winf = true, winRf = true;
            do
            {
                i++;
                var drawInfo = lc.Draw();
                LottoTicket lottoTicketR = new LottoTicket(true);
                LottoTicket lottoTicket = new LottoTicket(new List<byte>()
                {
                    5, 15, 26, 27, 34, 44
                }
                    );
                win = drawInfo.Equals(lottoTicket);
                winR = drawInfo.Equals(lottoTicketR);

                if (win && winf)
                {
                    winf = false;
                    Console.WriteLine("win: " + i);
                }
                if (winR && winRf)
                {
                    winRf = false;
                    Console.WriteLine("winR " + i);
                }
            } while (winf || winRf);

            //Console.WriteLine(i);
            //if(win)
            //    Console.WriteLine("win");
            //if (winR)
            //    Console.WriteLine("winR");
        }

        private void button10_Click(object sender, EventArgs e)
        {

            Queue<SuperLottoResultInfo> superLottoResultInfos = new Queue<SuperLottoResultInfo>(slc.History);
            slc.ClearHistory();

            for (int i = 0; i < 500; i++)
            {
                slc.AddRecordData(superLottoResultInfos.Dequeue());
            }

            Dictionary<int, int> starsCount = new Dictionary<int, int>();
            var history = slc.History;
            for (int i = 0; i < history.Count; i++)
            {
                var stars = history[i].BasicStars;
                for (int j = 0; j < stars.Count; j++)
                {
                    if(starsCount.TryGetValue(stars[j], out int cnt))
                    {
                        starsCount[stars[j]]++;
                    }
                    else
                    {
                        starsCount.Add(stars[j], 1);
                    }
                }
            }
            do
            {
                //var lastHistory = slc.LastHistory;

                #region 第一區
                int minCnt = starsCount.Min(r => r.Value);
                int minStar = starsCount.Where(r => r.Value == minCnt).ElementAt(0).Key;
                Console.WriteLine(minStar);
                //var basicStars = lastHistory.BasicStars;

                //for (int i = 0; i < basicStars.Count; i++)
                //{
                //    // 找出現同位置同號碼的下一期
                //    var stars = history.Where(r => r.BasicStars[i] == basicStars[i]);
                //    List<int> starsIndex = new List<int>();
                //    foreach (var star in stars)
                //    {
                //        starsIndex.Add(history.IndexOf(star));
                //    }

                //    // 找下一期出現最多次的尾數
                //    Dictionary<int, int> tailCount = new Dictionary<int, int>();
                //    for (int j = 0; j < starsIndex.Count - 1; j++)
                //    {
                //        var nextHistory = history[starsIndex[j] + 1];
                //        var nextBasicStars = nextHistory.BasicStars;
                //        for (int k = 0; k < nextBasicStars.Count; k++)
                //        {
                //            int tail = nextBasicStars[k];
                //            if (tailCount.TryGetValue(tail, out int cnt))
                //            {
                //                tailCount[tail]++;
                //            }
                //            else
                //            {
                //                tailCount.Add(tail, 1);
                //            }
                //        }
                //    }
                //    int maxCnt = tailCount.Max(r => r.Value);
                //    var maxtail = tailCount.Where(r => r.Value == maxCnt);
                //    Console.Write("tail: ");
                //    foreach (var t in maxtail)
                //    {
                //        Console.Write(t.Key + "-");                        
                //    }
                //    Console.WriteLine();
                //}
                #endregion

                #region 第二區
                #endregion

                #region 結果
                var info = superLottoResultInfos.Dequeue();
                Console.WriteLine(info.ToString());
                slc.AddRecordData(info);

                var stars = info.BasicStars;
                for (int j = 0; j < stars.Count; j++)
                {
                    if (starsCount.TryGetValue(stars[j], out int cnt))
                    {
                        starsCount[stars[j]]++;
                    }
                    else
                    {
                        starsCount.Add(stars[j], 1);
                    }
                }
                #endregion
            } while (superLottoResultInfos.Count != 0);
        }
    }
}
