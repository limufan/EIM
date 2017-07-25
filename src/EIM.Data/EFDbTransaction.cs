using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data
{
    public class EFDbTransaction : IDbTransaction
    {
        public EFDbTransaction(DbContextTransaction transaction)
        {
            this.Transaction = transaction;
        }

        public DbContextTransaction Transaction { private set; get; }

        public void Commit()
        {
            this.Transaction.Commit();
        }

        public void Dispose()
        {
            this.Transaction.Dispose();
        }

        public void Rollback()
        {
            this.Transaction.Rollback();
        }
    }
}
