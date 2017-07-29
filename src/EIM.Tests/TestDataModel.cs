using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests
{
    public class TestDataModel : IKeyProvider
    {
        public object GetKey()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public class TestBusinessModel : IKeyProvider
    {
        public object GetKey()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public class TestDataModel_1
    {

    }
}
