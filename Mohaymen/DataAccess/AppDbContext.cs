using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohaymen.DataAccess
{
    public class AppDbContext :DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ALI\\SQLEXPRESS;Database=mohaymen1;Integrated Security=True;Encrypt=Mandatory;TrustServerCertificate=True;");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                   
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender) 
                .WithMany(u => u.SentMessages) 
                .HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict); ; 

          
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver) 
                .WithMany(u => u.ReceivedMessages) 
                .HasForeignKey(m => m.ReceiverId).OnDelete(DeleteBehavior.Restrict); ; 
        }


    }
}
