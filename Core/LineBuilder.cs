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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counterpoint.Core
{
    public class LineBuilder
    {
        public bool Complete { get; private set; }

        private StringBuilder nextNote = new StringBuilder();
        public readonly List<Pitch> Pitches = new List<Pitch>();

        public void Add(char next)
        {
            if (next == ' ')
            {
                CompleteNote();
            }
            else if (next == 'x' || next == 'X')
            {
                CompleteNote();
                Complete = true;
            }
            else
            {
                nextNote.Append(next);
            }
        }

        private void CompleteNote()
        {
            string note = nextNote.ToString();
            nextNote = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(note))
            {
                if (Pitch.IsValidScientificNotation(note))
                {
                    Pitch p = new Pitch(note);
                    Pitches.Add(p);
                }
                else
                {
                    throw new InvalidNoteException(note);
                }
            }
        }
    }
}
