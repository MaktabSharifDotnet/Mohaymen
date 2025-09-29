using Mohaymen.DataAccess;
using Mohaymen.Enums;
using Mohaymen.Exceptions;
using Mohaymen.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.Services
{
    public class Service
    {
        private readonly UserRepository _userRepository;
        public Service(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Register(string username , string pass) 
        {
            User? user=_userRepository.GetUserByUsername(username);
            if (user!=null)
            {
                throw new UserAlreadyExistException("A user with the same username has already registered.");
            }
            var newUser = new User
            {
                Username = username,
                Password=pass,
                Status= StatusEnum.available
            };
            _userRepository.AddUser(newUser);
        }
 
    }
}
