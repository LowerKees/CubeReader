using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class MatchException : System.Exception
    {
        public MatchException() { }
        public MatchException(string message) : base(message) { }
        public MatchException(string message, Exception innerException) : base(message, innerException){ }
    }
}
