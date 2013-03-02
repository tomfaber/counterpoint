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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counterpoint.UnitTests
{
    [TestClass]
    public class LineBuilderTests
    {
        [TestMethod]
        public void CompleteFalseWhenCreated()
        {
            LineBuilder lb = new LineBuilder();
            Assert.IsFalse(lb.Complete);
        }

        [TestMethod]
        public void AddValidNote()
        {
            LineBuilder lb = new LineBuilder();
            lb.Add('C');
            lb.Add(' ');
            Assert.IsFalse(lb.Complete);
            Assert.AreEqual(1, lb.Pitches.Count);
            Assert.AreEqual(new Pitch("C"), lb.Pitches[0]);
        }

        [TestMethod]
        public void AddInvalidNote()
        {
            LineBuilder lb = new LineBuilder();
            try
            {
                lb.Add('C');
                lb.Add('w');
                lb.Add(' ');
                Assert.Fail();
            }
            catch (InvalidNoteException ine)
            {
                Assert.AreEqual("Cw", ine.Note);
            }
            // test that even after failure we can still add stuff.
            lb.Add('C');
            lb.Add('#');
            lb.Add(' ');
            Assert.AreEqual(1, lb.Pitches.Count);
            Assert.AreEqual(new Pitch("C#"), lb.Pitches[0]);
        }

        [TestMethod]
        public void AddValidNotesSeparatedBySpace()
        {
            LineBuilder lb = new LineBuilder();
            lb.Add('C');
            lb.Add(' ');
            lb.Add('D');
            lb.Add(' ');
            lb.Add('E');
            Assert.IsFalse(lb.Complete);
            Assert.AreEqual(2, lb.Pitches.Count); // E should not be added yet.
            Assert.AreEqual(new Pitch("C"), lb.Pitches[0]);
            Assert.AreEqual(new Pitch("D"), lb.Pitches[1]);
        }

        [TestMethod]
        public void CompletesWithX()
        {
            LineBuilder lb = new LineBuilder();
            lb.Add('C');
            lb.Add(' ');
            lb.Add('D');
            lb.Add('x');
            Assert.IsTrue(lb.Complete);
            Assert.AreEqual(2, lb.Pitches.Count);
            Assert.AreEqual(new Pitch("D"), lb.Pitches[1]);
        }

        [TestMethod]
        public void CompletesWithXAfterSpace()
        {
            LineBuilder lb = new LineBuilder();
            lb.Add('C');
            lb.Add(' ');
            lb.Add('D');
            lb.Add(' ');
            lb.Add('x');
            Assert.IsTrue(lb.Complete);
            Assert.AreEqual(2, lb.Pitches.Count);
            Assert.AreEqual(new Pitch("D"), lb.Pitches[1]);
        }
    }
}
