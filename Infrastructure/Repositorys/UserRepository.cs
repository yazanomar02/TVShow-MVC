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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(TVDbContext context) : base(context)
        {
        }


        public User CheckUser(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email && !u.IsDeleted);

            if (user is not null)
            {
                if(user.Password == password)
                    return user;
            }

            return null;
        }

        public async Task Delete(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user is not null)
            {
                user.IsDeleted = true;
            }

            context.Users.Update(user);
        }

        public User FindByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
