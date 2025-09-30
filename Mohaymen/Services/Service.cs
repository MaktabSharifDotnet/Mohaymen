using Mohaymen.DataAccess;
using Mohaymen.Enums;
using Mohaymen.Exceptions;
using Mohaymen.Manager;
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

        public void Register(string username , string password) 
        {
            User? userFromDb=_userRepository.GetUserByUsername(username);
            if (userFromDb!=null)
            {
                throw new UserAlreadyExistException("A user with this username has already registered.");
            }
            User user = new User 
            { 
              Username = username,
              Password = password,
              Status=StatusEnum.available,                       
            };
            _userRepository.AddUser(user);
        }
        public void Login(string username, string password) 
        {
            User? userFromDb = _userRepository.GetUserByUsername(username);
            if (userFromDb == null || userFromDb.Password!=password)
            {
                throw new UserNotFoundException("The username or password is incorrect.");
            }
            LocalStorage.Login(userFromDb);
        }
    }
}
