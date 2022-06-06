using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoCalculator
{
    public class LottoCal
    {
        List<LottoData> RecordDatas;
        ulong lAllPossibility;
        Random r;
        public LottoCal()
        {
            lAllPossibility = C(49, 6);
            r = new Random(Guid.NewGuid().GetHashCode());
            RecordDatas = new List<LottoData>();
        }

        public void AddRecordData(LottoData ld)
        {
            RecordDatas.Add(Sort(ld));
        }

        public static ulong C(ulong up, ulong down)
        {
            // 防呆
            if (down > up)
                return 0;

            // 減少計算量
            if (down > up / 2)
                down = up - down;
            
            if (down == 0)
                return 1;

            // 累乘
            ulong lUp = 1;
            ulong lDown = 1;
            for (ulong i = 0; i < down; i++)
            {
                lUp *= (up - i);
                lDown *= (down - i);               
            }

            return lUp / lDown;
        }

        public static LottoData Sort(LottoData ld)
        {
            List<byte> list = new List<byte>();
            list.Add(ld.Star1);
            list.Add(ld.Star2);
            list.Add(ld.Star3);
            list.Add(ld.Star4);
            list.Add(ld.Star5);
            list.Add(ld.Star6);
            list.Sort();
            LottoData ld_ = new LottoData();
            ld_.Star1 = list[0];
            ld_.Star2 = list[1];
            ld_.Star3 = list[2];
            ld_.Star4 = list[3];
            ld_.Star5 = list[4];
            ld_.Star6 = list[5];

            return ld_;
        }

        /// <summary>
        /// 大樂透指定星數機率
        /// </summary>
        /// <param name="star">中了幾星</param>
        /// <param name="special">有無中特別號</param>
        /// <returns></returns>
        public ulong WinningCount(byte star, byte special)
        {
            if (star > 6)
                return 0;
            if (special > 1)
                return 0;
            if (special == 1 && star > 5)
                return 0;

            // 6顆開出的號碼內選star顆
            // 1顆開出的號碼內選special顆
            // 剩下沒開出的號碼(42顆)內選沒中的數量(6-star-special顆)
            ulong up = C(6, star) * C(1, special) * C(42, 6ul - star - special);
            return up;
        }

        public string WinningRate(byte star, byte special)
        {
            var up = WinningCount(star, special);
            return up.ToString() + " / " + lAllPossibility.ToString();
        }

        public LottoData Draw()
        {
            List<byte> box = new List<byte>();
            for (byte i = 1; i <= 49; i++)
                box.Add(i);

            List<byte> stars = new List<byte>();
            for (byte i = 0; i < 6; i++)
            {
                byte index = Convert.ToByte(r.Next(1, box.Count));
                stars.Add(box[index]);
                box.RemoveAt(index);
            }

            stars.Sort();
            LottoData result = new LottoData();
            result.Star1 = stars[0];
            result.Star2 = stars[1];
            result.Star3 = stars[2];
            result.Star4 = stars[3];
            result.Star5 = stars[4];
            result.Star6 = stars[5];

            return result;
        }

        public LottoData DrawWithoutRecord()
        {
            bool flag = false;
            LottoData rtn = new LottoData();
            do
            {
                LottoData ld = Draw();

                flag = !RecordDatas.Contains(ld);
                if (flag)
                    rtn = ld;

            } while (!flag);

            return rtn;
        }

        public bool Contain(LottoData ld)
        {
            return RecordDatas.Contains(Sort(ld));
        }

        public bool Contain(byte star1, byte star2, byte star3, byte star4, byte star5, byte star6)
        {
            LottoData ld = new LottoData
            {
                Star1 = star1,
                Star2 = star2,
                Star3 = star3,
                Star4 = star4,
                Star5 = star5,
                Star6 = star6
            };
            return RecordDatas.Contains(Sort(ld));
        }

        public bool Contain(byte[] stars)
        {
            for (ulong i = 0; i < C(Convert.ToUInt64(stars.Length), 6); i++)
            {
                LottoData ld = new LottoData();
                //ld.Star1
            }

            return false;
        }
    }


    public struct LottoData
    {
        public byte Star1;
        public byte Star2;
        public byte Star3;
        public byte Star4;
        public byte Star5;
        public byte Star6;
        public byte Special;
        public string Date;
        public string Index;

        public bool Equals(LottoData ld)
        {
            //按需求定製自己需要的比較方式
            return (this.Star1 == ld.Star1 &&
                this.Star2 == ld.Star2 &&
                this.Star3 == ld.Star3 &&
                this.Star4 == ld.Star4 &&
                this.Star5 == ld.Star5 &&
                this.Star6 == ld.Star6);
        }

        public override string ToString()
        {
            return
                Star1.ToString() + "," +
                Star2.ToString() + "," +
                Star3.ToString() + "," +
                Star4.ToString() + "," +
                Star5.ToString() + "," +
                Star6.ToString() + "," +
                Special.ToString() + "," +
                Date + "," +
                Index + ",";
        }
    }
}
