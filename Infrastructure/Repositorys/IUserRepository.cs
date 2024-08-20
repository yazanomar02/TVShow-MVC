using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositorys
{
    public interface IUserRepository : IRepository<User>
    {
        User CheckUser(string email, string password);
        User FindByEmail (string email);
        Task Delete(Guid id);
    }
}
