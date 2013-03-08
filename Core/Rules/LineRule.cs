using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Counterpoint.Core.Rules
{
    public class LineRule
    {
        private readonly Func<List<Pitch>, IEnumerable<CounterpointError>> _checkMethod;
        private CounterpointErrorLevel _counterpointErrorLevel;

        // TODO: since all my code is manipulating lists and passing anonymous functions around, perhaps should make it F#.

        // for rules that return either zero or one errors.
        public LineRule(CounterpointErrorLevel errorLevel, Func<List<Pitch>,CounterpointError> checkMethod)
        {
            _checkMethod = (line) =>
            {
                var error = checkMethod(line);
                if (error != null)
                    return new List<CounterpointError> { error };
                return new List<CounterpointError>();
            };
        }

        public LineRule(CounterpointErrorLevel counterpointErrorLevel, Func<List<Pitch>,IEnumerable<CounterpointError>> func)
        {
            _counterpointErrorLevel = counterpointErrorLevel;
            _checkMethod = func;
        }

        public static IEnumerable<LineRule> All
        {
            get
            {
                yield return LineRules.ShouldntCoverMoreThanATenth;
                yield return LineRules.MelodicIntervals;
            }
        }

        // TODO: refactor so this takes a Line object, where Line object ctor takes the list and calculates melodic intervals.
        public IEnumerable<CounterpointError> Check(List<Pitch> line)
        {
            return _checkMethod(line);
        }

        public CounterpointErrorLevel ErrorLevel { get; protected set; }
    }

    public static class LineRules
    {
        // from http://humanities.uchicago.edu/classes/zbikowski/species.html
        public static readonly LineRule ShouldntCoverMoreThanATenth = new LineRule
            (
                CounterpointErrorLevel.Error,
                (line) => (line.Max().Value - line.Min().Value) > 16 ?
                        new CounterpointError("The overall range of the line, from " + line.Min().ScientificNotation + " to " + line.Max().ScientificNotation + " is greater than a tenth") : null
            );

        // from http://humanities.uchicago.edu/classes/zbikowski/species.html
        public static readonly LineRule MelodicIntervals = new LineRule
            (
                CounterpointErrorLevel.Error,
                (line) =>
                {
                    List<CounterpointError> foo = new List<CounterpointError>();
                    for (int i = 1; i < line.Count; i++)
                    {
                        Pitch p1 = line[i - 1];
                        Pitch p2 = line[i];
                        Interval interval = p2 - p1;

                        // note - Unisons (same note twice in a row) allowed if not in cantus firmus.
                        switch (interval)
                        {
                            case Interval.Tritone:
                            case Interval.MajorSeventh:
                            case Interval.MinorSeventh:
                                string direction = p1 < p2 ? " up to " : " down to ";
                                foo.Add(new CounterpointError("The interval from " + p1.ScientificNotation + direction + p2.ScientificNotation + " is not an allowed melodic interval."));
                                break;
                        }
                    }
                    return foo;
                }
            );


        // from http://humanities.uchicago.edu/classes/zbikowski/species.html
        // No more than two consecutive leaps are advised; any more and the line loses its melodic sense.
        // assuming "leap" means melodic interval greater than a major second.  
        // assuming it means in any direction.
        public static readonly LineRule NoMoreThanTwoConsecutiveLeaps = new LineRule
            (
                CounterpointErrorLevel.Error,
                (line) =>
                {
                    List<CounterpointError> foo = new List<CounterpointError>();
                    int consecutiveLeaps = 0;

                    // TODO: calculating the melodic intervals happens multiple times.  need to fix.
                    for (int i = 1; i < line.Count; i++)
                    {
                        Pitch p1 = line[i - 1];
                        Pitch p2 = line[i];
                        Interval interval = p2 - p1;


                        if (interval > Interval.WholeStep)
                        {
                            consecutiveLeaps++;
                        }
                        else
                        {
                            consecutiveLeaps = 0;
                        }

                        if (consecutiveLeaps == 3) // ==3, not >2, so that we only get one per.
                        {
                            foo.Add(new CounterpointError("There should be no more than two consecutive leaps."));
                        }
                    }
                    return foo;
                }
            );

    }
}
