using EIM.Data.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data.DataModelProviders
{
    public class UserDataModelProvider: EFDataModelProvider<UserDataModel>
    {
        public UserDataModelProvider(EIMDbContext dbContext)
            :base(dbContext)
        {

        }
    }
}
