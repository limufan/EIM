using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM
{
    [Serializable]
    public class DateTimeRange
    {
        public DateTimeRange()
        {
        }

        public DateTimeRange(DateTime? start, DateTime? end)
        {
            this.start = start;
            this.end = end;
        }

        public DateTime? start { set; get; }

        public DateTime? end { set; get; }

        public bool InRange(DateTime? date)
        {
            if (!date.HasValue)
            {
                return false;
            }

            if (this.end.HasValue && this.start.HasValue)
            {
                if (date.Value < start.Value || date.Value > this.end.Value)
                {
                    return false;
                }
            }
            else if (this.end.HasValue)
            {
                if (date.Value > this.end.Value)
                {
                    return false;
                }
            }
            else if (this.start.HasValue)
            {
                if (date.Value < start.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public bool InRange(DateTimeRange range)
        {
            if (this.InRange(range.start))
            {
                return true;
            }
            if (this.InRange(range.end))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            if (this.start.HasValue && this.end.HasValue)
            {
                return string.Format("{0}到{1}", this.start.Value.ToString("yyyy-MM-dd HH:mm"), this.end.Value.ToString("yyyy-MM-dd HH:mm"));
            }
            else if (this.start.HasValue)
            {
                return string.Format("{0}以后", this.start.Value.ToString("yyyy-MM-dd HH:mm"));
            }
            else if (this.end.HasValue)
            {
                return string.Format("{0}以前", this.end.Value.ToString("yyyy-MM-dd HH:mm"));
            }
            return "";
        }
    }
}
