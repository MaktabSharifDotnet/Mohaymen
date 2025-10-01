using Mohaymen.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.Manager
{
    public static class LocalStorage
    {
        public static User LoginUser { get; set; }
        public static void Login(User user) 
        {
          LoginUser = user;
        }
        public static void Logout() 
        {
            
            LoginUser = null;
        }
    }
}
