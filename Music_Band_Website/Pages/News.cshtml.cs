using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music_Band_Website.Model;

namespace Music_Band_Website.Pages
{
    public class NewsModel : PageModel
    {
        private readonly ApplicationDBContext context;
        public NewsModel(ApplicationDBContext _context)
        {
            context = _context;
        }

        public List<News> AllNews { get; set; }

        public void OnGet()
        {
            AllNews = context.News.OrderByDescending(n => n.Id).ToList();
        }
    }
}
