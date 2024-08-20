using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositorys
{
    public interface ILanguageRepository : IRepository<Language>
    {
        Task<Language?> FindLanguageByNameAsync(string name);
    }
}
