using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Tests
{
    public class ObjectEqualAsserter : ObjectComparer
    {
        public void AssertEqual(object expected, object actual)
        {
            Assert.AreEqual(true, this.Compare(expected, actual, 1));
        }
    }
}
