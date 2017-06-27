using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EIM.Data.Org
{
    [Table("org_user")]
    public class UserModel
    {
        public int Id { set; get; }

        [MaxLength(50)]
        [Index]
        public string Guid { set; get; }

        [MaxLength(50)]
        public string Code { set; get; }
    }
}
