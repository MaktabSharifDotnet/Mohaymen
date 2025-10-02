using Mohaymen.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.repositories
{
    public class MessageRepository
    {
        private readonly AppDbContext _context;
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddMessage(Message message) 
        {
           _context.Messages.Add(message);
           _context.SaveChanges();
        }
    }
}
