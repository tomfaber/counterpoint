using Counterpoint.Core.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counterpoint.UnitTests
{
    [TestClass]
    public class CounterpointErrorTests
    {
        [TestMethod]
        public void ErrorMessage()
        {
            const string expected = "Hi mom";
            CounterpointError e = new CounterpointError(expected);
            Assert.AreEqual(expected, e.ToString());
        }
    }
}
