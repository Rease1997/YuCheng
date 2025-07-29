using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotFix.Common.Utils
{
    public class TimeUtils
    {
        public static double OnDiffSeconds(DateTime startTime, DateTime endTime)
        {
            TimeSpan secondSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
            return secondSpan.TotalSeconds;
        }
        public static string SecondsTimestampToTime(string stamp)
        {
            double timestamp=double.Parse(stamp);
            System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); 
            System.DateTime dt = startTime.AddSeconds(timestamp);
            string t = dt.ToString("yyyy/MM/dd HH:mm:ss"); 
            return t;

        }
        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public static string MilSecondsTimestampToTime(string stamp)
        {
            double timestamp = double.Parse(stamp);
            System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            System.DateTime dt = startTime.AddMilliseconds(timestamp);
            string t = dt.ToString("yyyy/MM/dd HH:mm:ss");
            return t;

        }
        public static double CountDownTime()
        {
            System.TimeSpan st = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0); 
            return st.TotalSeconds;
        }
        public static double CurTimeToTimestamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
             
        }
        public static string CombineTime(int time)
        {
            return time < 10 ? "0" + time : time.ToString();
        }
        public static string CombineTimeDouble(int time)
        {
            if(time<10)
            {
                return "00" +  time;
            }else if(time<100)
            {
                return "0" + time;
            }
            else
            {
                return time.ToString();
            }
            
        }
    }
}
