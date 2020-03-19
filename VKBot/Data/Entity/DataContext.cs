using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKBot.Data.Initialize;
using VKBot.Model;

namespace VKBot.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base("VKDB")
        {
            Database.SetInitializer<DataContext>(new AccountInitializer());        
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasMany(p => p.Users)
                .WithMany(c => c.Groups);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<BlackListWord> BlackListWords { get; set; }

        public DbSet<Log> Log { get; set; }

        public DbSet<Group> Groups { get; set; }
    }
}
