using Domain.Models;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositorys
{
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        public LanguageRepository(TVDbContext context) : base(context)
        {
        }

        public async Task<Language?> FindLanguageByNameAsync(string name)
        {
            return await context.Languages
                .FirstOrDefaultAsync(l => l.Name == name);
        }
    }
}
