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
    public class TvShowRepository : GenericRepository<TVShow>, ITvShowRepository
    {
        public TvShowRepository(TVDbContext context) : base(context)
        {
        }

        public async Task<List<TVShow>> GetAllTVShowsWithDetailsAsync()
        {
            return await context.TVShows
                .Include(tv => tv.Attachment)
                .Include(tv => tv.Languages)
                .ToListAsync();
        }

        public TVShow GetTVShowWithDetails(Guid id)
        {
            return context.TVShows
                .Include(ts => ts.Languages)   // تحميل اللغات المرتبطة
                .Include(ts => ts.Attachment)  // تحميل المرفقات المرتبطة
                .FirstOrDefault(ts => ts.TVShowId == id);
        }
    }
}
