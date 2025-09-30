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
        public Service(UserRepository userRepository, MessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
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

        public void Login(string username, string pass) 
        {
            User? user = _userRepository.GetUserByUsername(username);
            if (user == null || user.Password != pass)
            {
                throw new UserNotFoundException("The username or password is incorrect.");
            }
            
            LocalStorage.Login(user);
        }

        public string ChangeStatus(StatusEnum newStatus) 
        {
            if (LocalStorage.LoginUser==null)
            {
                throw new NotLogInException("User is not logged in.");
            }
            if (LocalStorage.LoginUser.Status==newStatus)
            {
                return $"Your status is already '{newStatus}'. No changes were made.";
            }
            LocalStorage.LoginUser.Status = newStatus;
            _userRepository.UpdateUser(LocalStorage.LoginUser);

            return $"Your status has changed to {newStatus}.";
        }

        public List<User> SearchUsername(string username) 
        {
            if (LocalStorage.LoginUser == null)
            {
                throw new NotLogInException("User is not logged in.");
            }
           return _userRepository.SearchUsername(username);
        }

        public void SendMessage(string username , string text) 
        {
            if (LocalStorage.LoginUser == null)
            {
                throw new NotLogInException("User is not logged in.");
            }
            User? receiverUser = _userRepository.GetUserByUsername(username);
            if (receiverUser == null)
            {
                throw new UserNotFoundException("The username or password is incorrect.");
            }
            AddMessage(receiverUser, text);
        }
        public List<User> GetUsers() 
        {
          return _userRepository.GetUsers();
        }
        private void AddMessage(User receiverUser, string text) 
        {
            Message message = new Message
            {
                SenderId = LocalStorage.LoginUser.Id,
                ReceiverId = receiverUser.Id,
                Text = text
            };
            _messageRepository.Add(message);
        }

        public List<Message> Inbox() 
        {
            if (LocalStorage.LoginUser == null)
            {
                throw new NotLogInException("User is not logged in.");
            }
            
           return _messageRepository.Inbox(LocalStorage.LoginUser.Id);
        }
    }
}
