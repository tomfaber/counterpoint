using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Counterpoint.Core
{
    public class InvalidNoteException : Exception
    {
        public InvalidNoteException(string note)
        {
            Note = note;
        }

        public string Note { get; private set; }
    }
}
