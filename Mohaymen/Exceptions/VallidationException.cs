using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.Exceptions
{
    public class VallidationException : Exception
    {
        public VallidationException(string message):base(message) 
        {
            
        }
    }
}
