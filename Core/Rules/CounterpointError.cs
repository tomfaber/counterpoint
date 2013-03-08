using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Counterpoint.Core.Rules
{
    public class CounterpointError
    {
        private readonly string _description;

        public CounterpointError(string description)
        {
            _description = description;
        }

        public override string ToString()
        {
            return _description;
        }
    }
}
