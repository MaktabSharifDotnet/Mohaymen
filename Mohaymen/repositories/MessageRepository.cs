using Microsoft.EntityFrameworkCore;
using Mohaymen.DataAccess;
using Mohaymen.DTOs;
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

        public void Add(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public List<Message> Inbox(int userId) 
        {
           return _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ReceiverId == userId)
                .ToList();       
        }

        public List<SentMessageDto> Sent(int userId) 
        {
           return  _context.Messages
                .Where(m => m.SenderId == userId).Select(m => new SentMessageDto
                {
                    Username = m.Receiver.Username,
                    Text = m.Text
                })
                .ToList();
        }
    }
}
