using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.Exceptions
{
    internal class UserAlreadyExistException : VallidationException
    {
        public UserAlreadyExistException(string message) : base(message)
        {
        }
    }
}
