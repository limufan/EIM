using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM
{
    /// <summary>
    /// 服务单类型
    /// </summary>
    public class OptionsValueAttribute : Attribute
    {
        public virtual List<object> Options { set; get; }
    }

    /// <summary>
    /// 服务单类型
    /// </summary>
    public class FuwudanTypeValueAttribute : OptionsValueAttribute
    {
        public FuwudanTypeValueAttribute()
        {
            this.Options = new List<object>();
            this.Options.Add("安装");
            this.Options.Add("维修");
        }
    }
}
