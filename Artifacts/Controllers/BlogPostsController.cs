using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Artifacts.Models.Entities;
using System.Linq;
using Artifacts.Models.ViewModels;
using Artifacts.Services;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Artifacts.Data;

namespace Artifacts.Controllers
{
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext _db;
        private IFileStorageService _fileStorageService;

        public BlogPostsController(ApplicationDbContext db, IFileStorageService fileStorageService)
        {
            _db = db;
            _fileStorageService = fileStorageService;
        }

        // GET: BlogPosts
        public async Task<ActionResult> Index()
        {
            ViewBag.selectedItem = "blog";

            var viewModel = await _db.BlogPosts.Select(bp => new BlogPostViewModel()
            {
                Id = bp.Id,
                Body = bp.Body,
                Thumbnail = new BlogPostImageViewModel(_fileStorageService)
                {
                    Id = bp.Thumbnail.Id,
                    ImageType = bp.Thumbnail.ImageType
                },
                Banner = new BlogPostImageViewModel(_fileStorageService)
                {
                    Id = bp.BannerImage.Id,
                    ImageType = bp.BannerImage.ImageType
                },
                Created = bp.Created,
                Edited = bp.Edited,
                Tags = bp.Tags,
                Title = bp.Title
            })
            .ToListAsync();
            return View(viewModel);
        }

        // GET: BlogPosts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var blogPost = await _db.BlogPosts.Select(bp => new BlogPostViewModel()
            {
                Id = bp.Id,
                Title = bp.Title,
                Body = bp.Body,
                Thumbnail = new BlogPostImageViewModel(_fileStorageService)
                {
                    Id = bp.Thumbnail.Id,
                    ImageType = bp.Thumbnail.ImageType
                },
                Banner = new BlogPostImageViewModel(_fileStorageService)
                {
                    Id = bp.BannerImage.Id,
                    ImageType = bp.BannerImage.ImageType
                },
                Created = bp.Created,
                Edited = bp.Edited,
                Tags = bp.Tags,
            })
            .Where(bp => bp.Id == id.Value)
            .FirstOrDefaultAsync();

            if (blogPost == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public ActionResult Create()
        {
            var viewModel = new BlogPostViewModel();
            return View(viewModel);
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(
            "Id," +
            "Title," +
            "Body," +
            "Tags," +
            "Created," +
            "Edited")] BlogPostViewModel blogPostViewModel, IFormFile thumbnailBytesUpload)
        {
            var blogPost = new BlogPost();
            blogPost.ViewModelToNewEntity(blogPostViewModel);

            if (ModelState.IsValid)
            {
                _db.BlogPosts.Add(blogPost);
                await _db.SaveChangesAsync();
                if (thumbnailBytesUpload != null && thumbnailBytesUpload.Length > 0)
                {
                    await _fileStorageService.UploadOrOverwriteImage(blogPost.Thumbnail, thumbnailBytesUpload);
                }
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var blogPost = await _db.BlogPosts.Select(bp => new BlogPostViewModel()
            {
                Id = bp.Id,
                Title = bp.Title,
                Body = bp.Body,
                Thumbnail = new BlogPostImageViewModel(_fileStorageService) {
                    Id = bp.Thumbnail.Id,
                    ImageType = bp.Thumbnail.ImageType
                },
                Banner = new BlogPostImageViewModel(_fileStorageService)
                {
                    Id = bp.BannerImage.Id,
                    ImageType = bp.BannerImage.ImageType
                },
                Created = bp.Created,
                Edited = bp.Edited,
                Tags = bp.Tags,
            })
            .Where(bp => bp.Id == id.Value)
            .FirstOrDefaultAsync();

            if (blogPost == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(
            "Id," +
            "Title," +
            "Body," +
            "Tags," +
            "Created," +
            "Edited")] BlogPostViewModel blogPostViewModel, IFormFile thumbnailBytesUpload)
        {
            if (ModelState.IsValid)
            {
                var blogPost = await _db.BlogPosts
                    .Include(bp => bp.Thumbnail)
                    .Include(bp => bp.BannerImage)
                    .Where(bp => bp.Id == blogPostViewModel.Id)
                    .FirstOrDefaultAsync();

                if (blogPost == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                blogPost.ViewModelToExistingEntity(blogPostViewModel);

                _db.Entry(blogPost).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                if (thumbnailBytesUpload != null && thumbnailBytesUpload.Length > 0)
                {
                    await _fileStorageService.UploadOrOverwriteImage(blogPost.Thumbnail, thumbnailBytesUpload);
                }

                return RedirectToAction("Index");
            }

            return View(blogPostViewModel);
        }

        // GET: BlogPosts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            BlogPost blogPost = await _db.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BlogPost blogPost = await _db.BlogPosts.FindAsync(id);
            _db.BlogPosts.Remove(blogPost);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
