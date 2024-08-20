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
using TV_Program_Management.Models;
using Infrastructure.Repositorys;
using TV_Program_Management.Repo;

namespace TV_Program_Management.Controllers
{
    public class TVShowsController : Controller
    {
        private readonly ITvShowRepository tvShowRepository;
        private readonly IRepository<Attachment> attachmentRepository;
        private readonly ILanguageRepository languageRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TVShowsController(
            ITvShowRepository tvShowRepository,
            IRepository<Attachment> attachmentRepository,
            ILanguageRepository languageRepository,
            IWebHostEnvironment webHostEnvironment
            )
        {
            this.tvShowRepository = tvShowRepository;
            this.attachmentRepository = attachmentRepository;
            this.languageRepository = languageRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET: TVShows
        public async Task<IActionResult> Index()
        {
            return View(await tvShowRepository.GetAllAsync());
        }

        // GET: TVShows/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tVShow = await tvShowRepository.GetAsync(id.Value);

            if (tVShow == null)
            {
                return NotFound();
            }

            return View(tVShow);
        }

        // GET: TVShows/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TVShows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TVShowId,Title,ReleassDate,Rating,URL,IsDeleted")] TVShow tVShow)
        {
            if (ModelState.IsValid)
            {
                tVShow.TVShowId = Guid.NewGuid();
                await tvShowRepository.AddAsync(tVShow);
                await tvShowRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tVShow);
        }

        // GET: TVShows/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tVShow = await tvShowRepository.GetAsync(id.Value);

            if (tVShow == null)
            {
                return NotFound();
            }
            return View(tVShow);
        }

        // POST: TVShows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TVShowId,Title,ReleassDate,Rating,URL,IsDeleted")] TVShow tVShow)
        {
            if (id != tVShow.TVShowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tvShowRepository.Update(tVShow);
                    await tvShowRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TVShowExists(tVShow.TVShowId))
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
            return View(tVShow);
        }

        // GET: TVShows/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tVShow = await tvShowRepository.GetAsync(id.Value);

            if (tVShow == null)
            {
                return NotFound();
            }

            return View(tVShow);
        }

        // POST: TVShows/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tVShow = await tvShowRepository.GetAsync(id);

            if (tVShow != null)
            {
                tVShow.IsDeleted = true;
                tvShowRepository.Update(tVShow);
            }

            await tvShowRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TVShowExists(Guid id)
        {
            return await tvShowRepository.ExistsAsync(id);
        }


        [Authorize]
        [HttpPost]
        [Route("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTvShow(TvShowModel tvShowModel)
        {

            if (!ModelState.IsValid)
            {
                // للإشارة إلى أنه يجب فتح النافذة المنبثقة لعرض رسالة خطأ
                TempData["ShowPopupErrorModel"] = true; // TempData[""] تحافظ على البيانات عند استخدام RedirectToAction()
                return RedirectToAction("Index", "Home");
            }

            // تحقق من أن Languages ليست فارغة
            if (tvShowModel.Languages == null || !tvShowModel.Languages.Any())
            {
                TempData["ShowPopupErrorModel"] = true;
                return RedirectToAction("Index", "Home");
            }

            // التحقق من نوع الملف
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var typeImage = Path.GetExtension(tvShowModel.Image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(typeImage))
            {
                TempData["ShowPopupErrorImage"] = true;
                return RedirectToAction("Index", "Home");
            }


            var tvShow = new TVShow()
            {
                Title = tvShowModel.Title,
                ReleassDate = tvShowModel.ReleassDate,
                Rating = tvShowModel.Rating,
                URL = tvShowModel.URL,
                Languages = tvShowModel.Languages.Select(lang => new Language { Name = lang }).ToList()
            };


            await tvShowRepository.AddAsync(tvShow);
            await tvShowRepository.SaveChangesAsync();

            // تعيين اسم الصورة باستخدام TVShowId
            var nameImage = $"{tvShow.TVShowId}{typeImage}";


            // مسار التخزين داخل wwwroot/uploads
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            var pathImage = Path.Combine(uploadsFolder, nameImage);


            // حفظ الصورة على القرص
            using (var stream = new FileStream(pathImage, FileMode.Create))
            {
                await tvShowModel.Image.CopyToAsync(stream);
            }

            var attachment = new Attachment()
            {
                Name = nameImage,
                Path = pathImage,
                FileType = typeImage,
            };

            await attachmentRepository.AddAsync(attachment);


            // ربط
            tvShow.Attachment = attachment;

            tvShowRepository.Update(tvShow);
            await tvShowRepository.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize]
        [Route("update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTVShow(TVShowModelForUpdate tVShow)
        {
            if (!ModelState.IsValid)
            {
                TempData["ShowPopupErrorModel"] = true;
                return RedirectToAction("Index", "Home");
            }

            if (tVShow == null)
            {
                return BadRequest("TVShow is null");
            }

            // العثور على الكائن الحالي من قاعدة البيانات 
            var existingTVShow = tvShowRepository.GetTVShowWithDetails(tVShow.TVShowId);

            if (existingTVShow == null)
            {
                return NotFound("TV Show not found");
            }


            // تحديث الصورة إذا تم رفع صورة جديدة
            if (tVShow.Image != null)
            {
                var uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "uploads");

                // حذف الصورة القديمة إذا كانت موجودة
                if (existingTVShow.Attachment != null && !string.IsNullOrEmpty(existingTVShow.Attachment?.Path))
                {
                    var oldImagePath = Path.Combine(uploadsDir, existingTVShow.Attachment.Path);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);

                        existingTVShow.Attachment.IsDeleted = true;
                        attachmentRepository.Update(existingTVShow.Attachment);
                    }
                }

                // استخراج امتداد الملف 
                var fileType = Path.GetExtension(tVShow.Image.FileName)?.ToLower();

                // تكوين اسم الملف الفريد باستخدام TVShowId وامتداد الصورة
                var uniqueFileName = tVShow.TVShowId.ToString() + fileType;

                // تحديد مسار حفظ الملف
                var filePath = Path.Combine(uploadsDir, uniqueFileName);

                // حفظ الصورة الجديدة
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await tVShow.Image.CopyToAsync(fileStream);
                }

                // تحديث المسار في الكائن
                existingTVShow.Attachment = new Attachment()
                {
                    Name = uniqueFileName, // قد ترغب في استخدام uniqueFileName هنا أيضًا
                    Path = filePath,
                    FileType = fileType // نوع الملف
                };

                await attachmentRepository.AddAsync(existingTVShow.Attachment);
            }

            // تحديث خصائص الكائن
            existingTVShow.Title = tVShow.Title;
            existingTVShow.ReleassDate = tVShow.ReleassDate;
            existingTVShow.Rating = tVShow.Rating;
            existingTVShow.URL = tVShow.URL;

            // معالجة اللغات
            var updatedLanguages = new List<Language>();
            foreach (var languageName in tVShow.Languages)
            {
                // محاولة العثور على اللغة في قاعدة البيانات
                var language = await languageRepository.FindLanguageByNameAsync(languageName);
                if (language == null)
                {
                    // إذا لم تكن اللغة موجودة، نضيفها
                    language = new Language { Name = languageName };
                    await languageRepository.AddAsync(language);
                }
                updatedLanguages.Add(language);
            }

            existingTVShow.Languages = updatedLanguages;

            // تحديث العرض التلفزيوني
            tvShowRepository.Update(existingTVShow);
            await tvShowRepository.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize]
        [Route("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTVShow(TVShow tVShow)
        {
            if (tVShow is null)
            {
                return NotFound();
            }

            var existingTVShow =tvShowRepository.GetTVShowWithDetails(tVShow.TVShowId);

            if (existingTVShow != null)
            {
                existingTVShow.IsDeleted = true;
                tvShowRepository.Update(existingTVShow);
                await tvShowRepository.SaveChangesAsync();
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
