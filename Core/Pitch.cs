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

using System;

namespace Counterpoint.Core
{
    public class Pitch : IComparable, IComparable<Pitch>
    {
        // TODO: flats not supported yet.
        private string[,] LettersAndAccidentals = new[,] { 
                {"C", "" },
                {"C", "#"},
                {"D", "" },
                {"D", "#"},
                {"E", "" },
                {"F", "" },
                {"F", "#"},
                {"G", "" },
                {"G", "#"},
                {"A", "" },
                {"A", "#"},
                {"B", "" }
            };

        // TODO: override that handles sharps and flats, distinguishes between C# and Db, etc.
        public Pitch(int value)
        {
            Octave = value / 12;
            Value = value;

            int pitchWithinOctave = value % 12;
            Letter = LettersAndAccidentals[pitchWithinOctave, 0];
            Accidental = LettersAndAccidentals[pitchWithinOctave, 1];
        }

        /// <summary>
        /// Octave 4 is the octave beginning with middle C, number increases from B to C.  Per http://en.wikipedia.org/wiki/Scientific_pitch_notation
        /// </summary>
        public int Octave { get; private set; }

        /// <summary>
        /// An absolute value (so C4's value will be below C5's value)
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// e.g. C, D, E
        /// </summary>
        public string Letter { get; private set; }

        /// <summary>
        /// #, b, or empty string
        /// </summary>
        public string Accidental { get; private set; }

        /// <summary>
        /// "C" for middle c, "C#" for the C# above middle C.  "C5" or "C6" for other octaves.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Letter + Accidental + (Octave == 4 ? "" : Octave.ToString());
        }

        public static readonly Pitch MiddleC = new Pitch(4 * 12);

        #region equals overrides
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Pitch p = obj as Pitch;
            return (p != null && p.Value == this.Value);
        }

        public bool Equals(Pitch p)
        {
            return (p != null && p.Value == this.Value);
        }

        public static bool operator ==(Pitch a, Pitch b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Value == b.Value;
        }

        public static bool operator !=(Pitch a, Pitch b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Value;
        }
        #endregion

        #region Comparable implementation and overrides

        public int CompareTo(Pitch other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return 1;
            }

            return this.Value.CompareTo(other.Value);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            Pitch other = obj as Pitch; // avoid double casting 
            if (other == null)
            {
                throw new ArgumentException("A Pitch object is required for comparison.", "obj");
            }
            return this.CompareTo(other);
        }
        
        public static int Compare(Pitch left, Pitch right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }
            if (object.ReferenceEquals(left, null))
            {
                return -1;
            }
            return left.CompareTo(right);
        }

        public static bool operator <(Pitch left, Pitch right)
        {
            return (Compare(left, right) < 0);
        }

        public static bool operator >(Pitch left, Pitch right)
        {
            return (Compare(left, right) > 0);
        }

        public static bool operator <=(Pitch left, Pitch right)
        {
            return (Compare(left, right) <= 0);
        }

        public static bool operator >=(Pitch left, Pitch right)
        {
            return (Compare(left, right) >= 0);
        }

        #endregion

        #region intervals
        public static Pitch operator +(Pitch start, Interval interval)
        {
            return new Pitch(start.Value + (int)interval);
        }
        public static Pitch operator -(Pitch start, Interval interval)
        {
            return new Pitch(start.Value - (int)interval);
        }
        #endregion

        public Interval IntervalTo(Pitch other)
        {
            if (Equals(other))
            {
                return Interval.Unison;
            }
            else if (IsEquivalent(other))
            {
                return Interval.Octave;
            }
            return (Interval)(Math.Abs(other.Value - Value) % 12);
        }

        public bool IsEquivalent(Pitch other)
        {
            // TODO: what should we do if this pitch is D# and the other pitch is Eb?  Depends how this is used.
            return (other != null && other.Letter == Letter && other.Accidental == Accidental);
        }
    }
}