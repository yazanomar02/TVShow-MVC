using Domain.Models;
using Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositorys
{
    public class AttachmentRepository : GenericRepository<Attachment>
    {
        public AttachmentRepository(TVDbContext context) : base(context)
        {
        }
    }
}
