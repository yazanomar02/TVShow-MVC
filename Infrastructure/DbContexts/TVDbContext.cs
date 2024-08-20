using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Attachment = Domain.Models.Attachment;

namespace Infrastructure.DbContexts
{
    public class TVDbContext : DbContext
    {
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<TVShow> TVShows { get; set; }

        public DbSet<User> Users { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var onConfiguringString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = ManagementTV_DB";

            optionsBuilder.UseSqlServer(onConfiguringString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
