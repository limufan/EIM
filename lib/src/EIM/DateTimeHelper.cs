using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 几个月之前
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool BeforeMonth(DateTime? date, int month)
        {
            if (!date.HasValue)
            {
                return false;
            }
            return date.Value.Date <= DateTime.Now.AddMonths(month * -1).Date;
        }

        /// <summary>
        /// 几个月以内
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool InMonth(DateTime? date, int month)
        {
            if (!date.HasValue)
            {
                return false;
            }
            return date.Value.Date >= DateTime.Now.AddMonths(month * -1).Date;
        }

        /// <summary>
        /// 几天之前
        /// </summary>
        /// <param name="date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static bool BeforeDays(DateTime? date, int days)
        {
            if (!date.HasValue)
            {
                return false;
            }
            return date.Value.Date < DateTime.Now.AddDays(days * -1).Date;
        }

        /// <summary>
        /// 几天之内
        /// </summary>
        /// <param name="date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static bool InDays(DateTime? date, int days)
        {
            if (!date.HasValue)
            {
                return false;
            }
            return date.Value.Date >= DateTime.Now.AddDays(days * -1).Date;
        }

        /// <summary>
        /// 在第几天前
        /// </summary>
        /// <param name="date"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static bool AtBeforeDays(DateTime? date, int days)
        {
            if (!date.HasValue)
            {
                return false;
            }
            return date.Value.Date == DateTime.Now.AddDays(days * -1).Date;
        }

        /// <summary>
        /// 几小时之前
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool BeforeHours(DateTime? date, int hours)
        {
            if (!date.HasValue)
            {
                return false;
            }
            return date <= DateTime.Now.AddHours(hours * -1);
        }

        /// <summary>
        /// 几小时之内
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool InHours(DateTime? date, int hours)
        {
            if (!date.HasValue)
            {
                return false;
            }
            return date >= DateTime.Now.AddHours(hours * -1);
        }

        public static string Format(DateTime? date)
        {
            if (!date.HasValue)
            {
                return "";
            }
            string formatString = date.Value.ToString("yyyy-MM-dd");
            if(formatString == "1900-01-01")
            {
                formatString = ""; 
            }

            return formatString;
        }
    }
}
