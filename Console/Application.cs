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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counterpoint.ConsoleApp
{
    public class Application
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter cantus firmus, with spaces between notes.  Type x after the last note.");

            LineBuilder cantusFirmusBuilder = new LineBuilder();
            while (!cantusFirmusBuilder.Complete)
            {
                try
                {
                    cantusFirmusBuilder.Add(Console.ReadKey().KeyChar);
                }
                catch (InvalidNoteException ine)
                {
                    Console.WriteLine();
                    Console.WriteLine(ine.Note + " is invalid. Notes must be in scientific pitch notation, e.g. c, c#, Db, F3, F#5.");
                }
            }

            Console.WriteLine();
            Console.Write("That completes the cantus firmus.  For now, I only support two-part counterpoint, first species.  ");
            Console.Write("Write a line to go above or below the cantus firmus.  Type a for above, b for below: ");
            int cantusFirmusPosition = -1;
            while (cantusFirmusPosition < 0)
            {
                char choice = Console.ReadKey().KeyChar;
                if (choice == 'a')
                {
                    cantusFirmusPosition = 0;
                }
                else if (choice == 'b')
                {
                    cantusFirmusPosition = 1;
                }
            }
        }
    }
}
