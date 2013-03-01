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

namespace Counterpoint.Core
{
    public enum Interval
    {
        Unison = 0,
        HalfStep = 1,
        WholeStep = 2,
        MinorThird = 3,
        MajorThird = 4,
        Fourth = 5,
        Tritone = 6, // yikes!
        Fifth = 7,
        MinorSixth = 8,
        MajorSixth = 9,
        MinorSeventh = 10,
        MajorSeventh = 11,
        Octave = 12
    }
}
