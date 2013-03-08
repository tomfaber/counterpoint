using Counterpoint.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Counterpoint.Core
{
    public class RuleSet
    {
        List<CounterpointError> _errors = new List<CounterpointError>();
        List<CounterpointError> _warnings = new List<CounterpointError>();

        public void Validate(List<Pitch> line)
        {

            foreach (LineRule lr in LineRule.All)  // TODO: refactor All into some other static class.
            {
                IEnumerable<CounterpointError> allViolations = lr.Check(line);
                switch (lr.ErrorLevel)
                {
                    case CounterpointErrorLevel.Error:
                        _errors.AddRange(allViolations);
                        break;
                    case CounterpointErrorLevel.Warning:
                        _warnings.AddRange(allViolations);
                        break;
                }
            }
        }

        public IEnumerable<CounterpointError> Errors { get { return _errors; } }

        public IEnumerable<CounterpointError> Warnings { get { return _warnings; } }

        public bool Valid { get { return _errors.Count + _warnings.Count == 0; } }
    }
}
