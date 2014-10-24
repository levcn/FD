using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STComponse
{
    public class PException : Exception
    {
        public PException(string message) : base(message)
        {}
    }
}
