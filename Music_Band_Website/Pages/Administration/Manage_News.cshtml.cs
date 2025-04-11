using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;
using Restaurant.Model;
using System.IO;
using System.Threading.Tasks;

namespace Music_Band_Website.Pages.Administration
{
    public class Manage_NewsModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public Manage_NewsModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<News> NewsList { get; set; } = new List<News>();

        [BindProperty]
        public News EditNews { get; set; }

        public async Task<IActionResult> OnGetAsync(int? editId)
        {
            NewsList = await _context.News.ToListAsync();

            if (editId.HasValue)
            {
                EditNews = await _context.News.FindAsync(editId.Value);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(IFormFile ImageFile)
        {
            var newsToUpdate = await _context.News.FindAsync(EditNews.Id);
            if (newsToUpdate == null) return NotFound();

            newsToUpdate.Title = EditNews.Title;
            newsToUpdate.Description = EditNews.Description;
            newsToUpdate.Details = EditNews.Details;

            if (ImageFile != null)
            {
                var filePath = Path.Combine("wwwroot/uploads", ImageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                newsToUpdate.Image = "/uploads/" + ImageFile.FileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _context.News.Remove(_context.News.Find(id)!);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddAsync(IFormFile ImageFile)
        {
            if (ImageFile == null)
            {
                ModelState.AddModelError("", "Image is required.");
                return Page();
            }

            var filePath = Path.Combine("wwwroot/uploads", ImageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            var newNews = new News
            {
                Title = Request.Form["Title"],
                Description = Request.Form["Description"],
                Details = Request.Form["Details"],
                Image = "/uploads/" + ImageFile.FileName
            };

            _context.News.Add(newNews);
            await _context.SaveChangesAsync();
            _ = NotifyUsersAsync();
            return RedirectToPage();
        }

        private async Task NotifyUsersAsync()
        {
            var tasks = _context.Subscriptions
                .Select(s => Mailer.SendMailAsync(new User { Email = s.Email }, "News"))
                .ToList();

            await Task.WhenAll(tasks);
        }
    }
}
