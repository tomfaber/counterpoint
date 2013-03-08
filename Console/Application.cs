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
using Counterpoint.Core.Rules;
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
            Console.WriteLine("Enter a line of music, with spaces between notes.  Type x or hit enter after the last note.");

            LineBuilder lineBuilder = new LineBuilder();
            while (!lineBuilder.Complete)
            {
                try
                {
                    lineBuilder.Add(Console.ReadKey().KeyChar);
                }
                catch (InvalidNoteException ine)
                {
                    Console.WriteLine();
                    Console.WriteLine(ine.Note + " is invalid. Notes must be in scientific pitch notation, e.g. c, c#, Db, F3, F#5.");
                }
            }

            Console.WriteLine();

            // TODO: refactor, perhaps so I can do something like:
            //    RulesResult rr = Rules.SingleLine.Validate(lineBuilder.Pitches);

            RuleSet rules = new RuleSet();

            rules.Validate(lineBuilder.Pitches);

            if (rules.Valid)
            {
                Console.WriteLine("Success!  Congratulations.");
            }
            else
            {
                if (rules.Errors.Count() > 0)
                {
                    Console.WriteLine("Errors:");
                    foreach (CounterpointError error in rules.Errors)
                    {
                        Console.WriteLine(error.ToString());
                    }
                }
                if (rules.Warnings.Count() > 0)
                {
                    Console.WriteLine("Warnings:");
                    foreach (CounterpointError warning in rules.Warnings)
                    {
                        Console.WriteLine(warning.ToString());
                    }
                }
            }

            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        } 
    }
}
