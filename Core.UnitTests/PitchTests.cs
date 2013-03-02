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
using System.Collections.Generic;

namespace Counterpoint.UnitTests
{
    [TestClass]
    public class PitchTests
    {
        static Random r = new Random();
        Pitch middleC = new Pitch("C");

        [TestMethod]
        public void CheckToStrings()
        {
            foreach (Pitch p in AllPitches())
            {
                Console.WriteLine(p.ToString());
            }
        }

        [TestMethod]
        public void PitchMiddleC()
        {
            Assert.AreEqual(4, middleC.Octave);
            Assert.AreEqual("C", middleC.Letter);
            Assert.AreEqual("C", middleC.ScientificNotation);
        }

        [TestMethod]
        public void PitchEquals()
        {
            Pitch p1 = RandomPitch();
            Pitch p2 = new Pitch(p1.ScientificNotation);
            Assert.IsTrue(p1 == p2);
            Assert.IsFalse(p1 != p2);
            Assert.IsTrue(p1.Equals(p2));
            Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
        }

        [TestMethod]
        public void PitchNotEquals()
        {
            Pitch p1 = RandomPitch();
            Pitch p2 = p1 + (Interval)r.Next(1,12);
            Assert.IsFalse(p1 == p2);
            Assert.IsTrue(p1 != p2);
            Assert.IsFalse(p1.Equals(p2));
            Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
        }

        [TestMethod]
        public void PitchEquivalentForUnison()
        {
            Pitch p1 = RandomPitch();
            Pitch p2 = new Pitch(p1.ScientificNotation);
            Assert.IsTrue(p1.IsEquivalent(p2));
        }

        [TestMethod]
        public void PitchEquivalentForOctave()
        {
            Pitch p1 = RandomPitch();
            Pitch p2 = p1 + Interval.Octave;
            Assert.IsTrue(p1.IsEquivalent(p2));
        }

        [TestMethod]
        public void PitchEquivalentRandom()
        {
            Pitch p1 = RandomPitch();
            Pitch p2 = RandomPitch();
            Assert.AreEqual((p2.Value - p1.Value) % 12 == 0, p1.IsEquivalent(p2), p1.ToString() + " " + p2.ToString());
        }

        [TestMethod]
        public void PitchCompare()
        {
            Pitch lower = RandomPitch();
            Pitch higher = lower + Interval.HalfStep;

            Assert.IsTrue(higher > lower);
            Assert.IsFalse(higher < lower);
            Assert.IsFalse(lower > higher);
            Assert.IsTrue(lower < higher);

            Assert.IsTrue(lower <= higher);
            Assert.IsFalse(lower >= higher);
            Assert.IsTrue(lower <= new Pitch(lower.ScientificNotation));
        }

        [TestMethod]
        public void AddingIntervals()
        {
            Pitch middleG = middleC + Interval.Fifth;
            Assert.AreEqual("G", middleG.Letter);
            Assert.AreEqual("", middleG.Accidental);
            Assert.AreEqual(middleC.Octave, middleG.Octave);
        }

        [TestMethod]
        public void SubtractingIntervals()
        {
            Pitch lowF = middleC - Interval.Fifth;
            Assert.AreEqual("F", lowF.Letter);
            Assert.AreEqual("", lowF.Accidental);
            Assert.AreEqual(middleC.Octave - 1, lowF.Octave);
        }

        [TestMethod]
        public void CalculateIntervals()
        {
            Pitch p1 = RandomPitch();
            foreach (Pitch p2 in AllPitches())
            {
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
                    Pitch p2plusI = p2 + i;
                    Assert.IsTrue(p1.IsEquivalent(p2plusI), "p1: " + p1.ToString() + " p2: " + p2.ToString() + " i: " + i.ToString() + " p2plusI: " + p2plusI);
                }
            }
        }

        private IEnumerable<Pitch> AllPitches()
        {
            for (int octaveIndex = 0; octaveIndex < ValidOctaves.Length; octaveIndex++)
            {
                for (int letterIndex = 0; letterIndex < ValidLetters.Length; letterIndex++)
                {
                    for (int accidentalIndex = 0; accidentalIndex < ValidAccidentals.Length; accidentalIndex++)
                    {
                        yield return new Pitch(ValidLetters[letterIndex] + ValidAccidentals[accidentalIndex] + ValidOctaves[octaveIndex]);
                    }
                }
            }
        }

        [TestMethod]
        public void ValidPitchNotation()
        {
            Assert.IsTrue(Pitch.IsValidScientificNotation("C"));
            Assert.IsTrue(Pitch.IsValidScientificNotation("A#"));
            Assert.IsTrue(Pitch.IsValidScientificNotation("Gb3"));
            Assert.IsTrue(Pitch.IsValidScientificNotation("F4"));
        }

        [TestMethod]
        public void InvalidPitchNotation()
        {
            Assert.IsFalse(Pitch.IsValidScientificNotation(""));
            Assert.IsFalse(Pitch.IsValidScientificNotation("A##"));
            Assert.IsFalse(Pitch.IsValidScientificNotation("3Gb"));
            Assert.IsFalse(Pitch.IsValidScientificNotation("FC"));
        }

        [TestMethod]
        public void PitchNotationCaseInsensitive()
        {
            Pitch p = new Pitch("C");
            Pitch p2 = new Pitch("c");
            Assert.AreEqual(p, p2);
        }

        [TestMethod]
        public void CIsSameAsMiddleCIsSameAsC4()
        {
            Pitch p = new Pitch("C");
            Pitch p1 = new Pitch("C4");
            Assert.AreEqual(middleC, p);
            Assert.AreEqual(middleC, p1);
        }

        string[] ValidLetters = new[] { "C", "D", "E", "F", "G", "A", "B" };
        string[] ValidAccidentals = new[] { "b", "", "#" };
        string[] ValidOctaves = new[] { "0", "1", "2", "3", "4", "", "5", "6", "7" };

        private Pitch RandomPitch()
        {
            string letter = ValidLetters[r.Next(ValidLetters.Length)];
            string accidental = ValidAccidentals[r.Next(ValidAccidentals.Length)];
            string octave = ValidOctaves[r.Next(ValidOctaves.Length)];

            return new Pitch(letter + accidental + octave);
        }


        [TestMethod]
        public void CreateAllPitchesWithNotation()
        {
            for (int octave = 0; octave < 7; octave++)
            {
                for (int letterIndex = 0; letterIndex < ValidLetters.Length; letterIndex++)
                {
                    string naturalPitch = ValidLetters[letterIndex] + octave.ToString();
                    string sharpPitch = ValidLetters[letterIndex] + "#" + octave.ToString();
                    string flatPitch = ValidLetters[letterIndex] + "b" + octave.ToString();

                    Pitch natural = new Pitch(naturalPitch);
                    Pitch sharp = new Pitch(sharpPitch);
                    Pitch flat = new Pitch(flatPitch); 

                    Assert.AreEqual(1, natural.Value - flat.Value);
                    Assert.AreEqual(1, sharp.Value - natural.Value);

                    Pitch naturalLower = new Pitch(naturalPitch.ToLowerInvariant());
                    Assert.AreEqual(natural, naturalLower);
                    Pitch sharpLower = new Pitch(sharpPitch.ToLowerInvariant());
                    Assert.AreEqual(sharp, sharpLower);
                }
            }
        }
    }
}
