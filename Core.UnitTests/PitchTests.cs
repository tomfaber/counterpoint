/* Copyright 2013 Tom Faber

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.  */

using Counterpoint.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Counterpoint.UnitTests
{
    [TestClass]
    public class PitchTests
    {
        static Random r = new Random();

        [TestMethod]
        public void PianosWorthOfPitches()
        {
            for (int i = 0; i < 89; i++)
            {
                Pitch p = new Pitch(i);
                Console.WriteLine(p.ToString());
            }
        }

        [TestMethod]
        public void PitchMiddleC()
        {
            Pitch middleC = Pitch.MiddleC;
            Assert.AreEqual(4, middleC.Octave);
            Assert.AreEqual("C", middleC.Letter);
            Assert.AreEqual("C", middleC.ToString());
        }

        [TestMethod]
        public void PitchEquals()
        {
            Pitch p1 = new Pitch(r.Next(89));
            Pitch p2 = new Pitch(p1.Value);
            Assert.IsTrue(p1 == p2);
            Assert.IsFalse(p1 != p2);
            Assert.IsTrue(p1.Equals(p2));
            Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
        }

        [TestMethod]
        public void PitchNotEquals()
        {
            Pitch p1 = new Pitch(r.Next(89));
            Pitch p2 = new Pitch(p1.Value + r.Next(1,20));
            Assert.IsFalse(p1 == p2);
            Assert.IsTrue(p1 != p2);
            Assert.IsFalse(p1.Equals(p2));
            Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
        }

        [TestMethod]
        public void PitchEquivalentForUnison()
        {
            Pitch p1 = new Pitch(r.Next(89));
            Pitch p2 = new Pitch(p1.Value);
            Assert.IsTrue(p1.IsEquivalent(p2));
        }

        [TestMethod]
        public void PitchEquivalentForOctave()
        {
            Pitch p1 = new Pitch(r.Next(89));
            Pitch p2 = new Pitch(p1.Value + 12);
            Assert.IsTrue(p1.IsEquivalent(p2));
        }

        [TestMethod]
        public void PitchEquivalentRandom()
        {
            Pitch p1 = new Pitch(r.Next(89));
            for (int i = 0; i < 89; i++)
            {
                Pitch p2 = new Pitch(i);
                Assert.AreEqual((p2.Value - p1.Value) % 12 == 0, p1.IsEquivalent(p2));
            }
        }

        [TestMethod]
        public void PitchCompare()
        {
            Pitch lower = new Pitch(r.Next(89));
            Pitch higher = new Pitch(lower.Value + r.Next(1,20));

            Assert.IsTrue(higher > lower);
            Assert.IsFalse(higher < lower);
            Assert.IsFalse(lower > higher);
            Assert.IsTrue(lower < higher);

            Assert.IsTrue(lower <= higher);
            Assert.IsFalse(lower >= higher);
            Assert.IsTrue(lower <= new Pitch(lower.Value));
        }

        [TestMethod]
        public void AddingIntervals()
        {
            Pitch middleC = Pitch.MiddleC;
            Pitch middleG = Pitch.MiddleC + Interval.Fifth;
            Assert.AreEqual("G", middleG.Letter);
            Assert.AreEqual("", middleG.Accidental);
            Assert.AreEqual(middleC.Octave, middleG.Octave);
        }

        [TestMethod]
        public void SubtractingIntervals()
        {
            Pitch middleC = Pitch.MiddleC;
            Pitch lowF = Pitch.MiddleC - Interval.Fifth;
            Assert.AreEqual("F", lowF.Letter);
            Assert.AreEqual("", lowF.Accidental);
            Assert.AreEqual(middleC.Octave - 1, lowF.Octave);
        }

        [TestMethod]
        public void CalculateIntervals()
        {
            Pitch p1 = new Pitch(r.Next(89));
            for (int v2 = 0; v2 < 89; v2++)
            {
                Pitch p2 = new Pitch(v2);

                Interval i = p1.IntervalTo(p2);

                if (p1 == p2)
                {
                    Assert.AreEqual(Interval.Unison, i);
                }
                else if (p1.IsEquivalent(p2))
                {
                    Assert.AreEqual(Interval.Octave, i);
                }
                else if (p1 < p2)
                {
                    Assert.IsTrue(p2.IsEquivalent(p1 + i));
                }
                else
                {
                    Assert.IsTrue(p1.IsEquivalent(p2 + i));
                }
            }
        }
    }
}
