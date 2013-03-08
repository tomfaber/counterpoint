using Counterpoint.Core;
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
    public class LineRuleTests
    {
        [TestMethod]
        public void RangeIsATenth()
        {
            List<Pitch> line = CreateTestLine("C D E F G A B C5 D5 E5");
            var errors = LineRules.ShouldntCoverMoreThanATenth.Check(line);
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void RangeIsAnEleventh()
        {
            List<Pitch> line = CreateTestLine("C D E F G A B C5 D5 F5");
            var errors = LineRules.ShouldntCoverMoreThanATenth.Check(line);
            Assert.AreEqual(1, errors.Count());
        }

        [TestMethod]
        public void FourthAllowedAsMelodicInterval()
        {
            List<Pitch> line = CreateTestLine("A D");
            var errors = LineRules.MelodicIntervals.Check(line);
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void MinorSeventhNotAllowedAsMelodicInterval()
        {
            List<Pitch> line = CreateTestLine("A5 G6");
            var errors = LineRules.MelodicIntervals.Check(line);
            Assert.AreEqual(1, errors.Count());
        }

        [TestMethod]
        public void CanHaveMultipleMelodicIntervalErrors()
        {
            var line = CreateTestLine("B C F#");
            var errors = LineRules.MelodicIntervals.Check(line);
            Assert.AreEqual(2, errors.Count());
        }

        [TestMethod]
        public void TooManyLeaps()
        {
            var line = CreateTestLine("C2 E2 G2 B2");
            var errors = LineRules.NoMoreThanTwoConsecutiveLeaps.Check(line);
            Assert.AreEqual(1, errors.Count());
        }

        [TestMethod]
        public void JustEnoughLeaps()
        {
            var line = CreateTestLine("C2 E2 G2 F2 E2 D2 C2 E2 G2 A2");
            var errors = LineRules.NoMoreThanTwoConsecutiveLeaps.Check(line);
            Assert.AreEqual(0, errors.Count());
        }

        private List<Pitch> CreateTestLine(string p)
        {
            LineBuilder lb = new LineBuilder();
            foreach (char c in p.ToCharArray())
            {
                lb.Add(c);
            }
            lb.Add('x');
            return lb.Pitches;
        }
    }
}
