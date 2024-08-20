using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Repositorys;

namespace TV_Program_Management.Controllers
{
    public class LanguagesController : Controller
    {
        private readonly IRepository<Language> languageRepository;

        public LanguagesController(IRepository<Language> languageRepository)
        {
            this.languageRepository = languageRepository;
        }

        // GET: Languages
        public async Task<IActionResult> Index()
        {
            return View(await languageRepository.GetAllAsync());
        }

        // GET: Languages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await languageRepository.GetAsync(id.Value);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // GET: Languages/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LanguageId,Name,IsDeleted")] Language language)
        {
            if (ModelState.IsValid)
            {
                language.LanguageId = Guid.NewGuid();
                await languageRepository.AddAsync(language);
                await languageRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Languages/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await languageRepository.GetAsync(id.Value);
            if (language == null)
            {
                return NotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LanguageId,Name,IsDeleted")] Language language)
        {
            if (id != language.LanguageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    languageRepository.Update(language);
                    await languageRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await LanguageExists(language.LanguageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Languages/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await languageRepository.GetAsync(id.Value);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var language = await languageRepository.GetAsync(id);
            if (language != null)
            {
                language.IsDeleted = true;
                languageRepository.Update(language);
            }

            await languageRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool>LanguageExists(Guid id)
        {
            return await languageRepository.ExistsAsync(id);
        }
    }
}
