using Domain.Models;
using Infrastructure.Repositorys;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TV_Program_Management.Models;
using TV_Program_Management.Repo;

namespace TV_Program_Management.Components
{
    public class TVShowListViewComponent : ViewComponent
    {
        private readonly ITvShowRepository tvShowRepository;

        public TVShowListViewComponent(ITvShowRepository tvShowRepository)
        {
            this.tvShowRepository = tvShowRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tvShows = await tvShowRepository.GetAllTVShowsWithDetailsAsync();

            if (tvShows == null || !tvShows.Any())
            {
                return View(new TVShowComponentModel());
            }

            var model = new TVShowComponentModel
            {
                TVShows = tvShows
            };

            return View(model);
        }
    }
}
