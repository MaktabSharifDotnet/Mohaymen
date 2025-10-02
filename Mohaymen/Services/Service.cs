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
        private readonly MessageRepository _messageRepository;
        public Service(UserRepository userRepository , MessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
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

        public string ChangeStatus(StatusEnum statusEnum) 
        {
            if (LocalStorage.LoginUser==null)
            {
                throw new NotLogInException("User is not logged in.");
            }
            if (LocalStorage.LoginUser.Status == statusEnum)
            {
                return $"Your current situation is {LocalStorage.LoginUser.Status}.";
            }
            else 
            {
                LocalStorage.LoginUser.Status = statusEnum;
                _userRepository.UpdateUser(LocalStorage.LoginUser);
                return $"The situation changed to {statusEnum}.";
            }
        }

        public List<User> Search(string username) 
        {
           return _userRepository.SearchUsername(username);
        }

        public void SendMessage(string receiver, string text) 
        {
            if (LocalStorage.LoginUser == null)
            {
                throw new NotLogInException("User is not logged in.");
            }
            User? user=_userRepository.GetUserByUsername(receiver);
            if (user == null) 
            {
                throw new UserNotFoundException("There is no user with this username.");
            }
            Message message = new Message 
            {
                ReceiverId = user.Id,
                Text = text,
                SenderId = LocalStorage.LoginUser.Id     
            };
            _messageRepository.AddMessage(message);
        }
        public List<User> GetUsers() 
        {
          return _userRepository.GetUsers();
        }
    }
}
