using Microsoft.EntityFrameworkCore;
using Mohaymen.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.repositories
{
    
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? GetUserByUsername(string username) 
        {
           return _context.Users.FirstOrDefault(u=>u.Username == username);
        
        }
        public List<User> SearchUsername(string username) 
        {
           return _context.Users.Where(u=>u.Username.Contains(username)).ToList();
        }

        public void AddUser(User user) 
        {
           _context.Users.Add(user);
           _context.SaveChanges();
        }

        public void UpdateUser(User user) 
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public List<User> GetUsers() 
        {
          return  _context.Users.ToList();
        
        }

        public User? GetUser(int id) 
        {
           return _context.Users.FirstOrDefault(u=>u.Id==id);
        }
    }
}
