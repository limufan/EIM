using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
