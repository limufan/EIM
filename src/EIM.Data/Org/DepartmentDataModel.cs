using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data.Org
{
    [Table("org_department")]
    public class DepartmentDataModel : IKeyProvider
    {
        public int Id { set; get; }

        [MaxLength(50)]
        [Index]
        public string Guid { set; get; }

        [MaxLength(50)]
        public string Code { set; get; }

        public object GetKey()
        {
            return this.Id;
        }
    }
}
