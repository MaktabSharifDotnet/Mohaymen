using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.Exceptions
{
    public class NotLogInException : VallidationException
    {
        public NotLogInException(string message) : base(message)
        {
        }
    }
}
