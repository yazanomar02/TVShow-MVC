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
    public class AttachmentsController : Controller
    {
        private readonly IRepository<Attachment> attachmentRepository;
        private readonly IRepository<TVShow> tvShowRepository;

        public AttachmentsController(
            IRepository<Attachment> attachmentRepository,
            IRepository<TVShow> tvShowRepository
            )
        {
            this.attachmentRepository = attachmentRepository;
            this.tvShowRepository = tvShowRepository;
        }

        // GET: Attachments
        public async Task<IActionResult> Index()
        {
            var tVDbContext = await attachmentRepository.GetAllAsync(true);
            return View(tVDbContext);
        }

        // GET: Attachments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await attachmentRepository.GetAsync(id.Value, true);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // GET: Attachments/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewData["TVShowId"] = new SelectList(await tvShowRepository.GetAllAsync(), "TVShowId", "ReleassDate");
            return View();
        }


        

        // POST: Attachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttachmentId,Name,Path,FileType,IsDeleted,TVShowId")] Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                attachment.AttachmentId = Guid.NewGuid();
                await attachmentRepository.AddAsync(attachment);
                await attachmentRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TVShowId"] = new SelectList(await tvShowRepository.GetAllAsync(), "TVShowId", "ReleassDate", attachment.TVShowId);
            return View(attachment);
        }

        // GET: Attachments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await attachmentRepository.GetAsync(id.Value);
            if (attachment == null)
            {
                return NotFound();
            }
            ViewData["TVShowId"] = new SelectList(await tvShowRepository.GetAllAsync(), "TVShowId", "ReleassDate", attachment.TVShowId);
            return View(attachment);
        }

        // POST: Attachments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AttachmentId,Name,Path,FileType,IsDeleted,TVShowId")] Attachment attachment)
        {
            if (id != attachment.AttachmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    attachmentRepository.Update(attachment);
                    await attachmentRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AttachmentExists(attachment.AttachmentId))
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
            ViewData["TVShowId"] = new SelectList(await tvShowRepository.GetAllAsync(), "TVShowId", "ReleassDate", attachment.TVShowId);
            return View(attachment);
        }

        // GET: Attachments/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachment = await attachmentRepository.GetAllAsync(true);
            if (attachment == null)
            {
                return NotFound();
            }

            return View(attachment);
        }

        // POST: Attachments/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attachment = await attachmentRepository.GetAsync(id);
            if (attachment != null)
            {
                attachment.IsDeleted = true;
                attachmentRepository.Update(attachment);
            }

            await attachmentRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AttachmentExists(Guid id)
        {
            return await attachmentRepository.ExistsAsync(id);
        }
    }
}
