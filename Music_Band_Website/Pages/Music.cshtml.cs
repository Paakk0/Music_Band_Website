using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music_Band_Website.Pages
{
    public class MusicModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public List<Song> Songs { get; set; }
        public List<Music_Type> MusicTypes { get; set; }
        public List<int> FavoriteSongIds { get; set; }

        public User User { get; set; }

        public MusicModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int method { get; set; }
        [BindProperty]
        public int id { get; set; }

        public void OnGet()
        {
            User = HttpContext.Session.GetObject<User>("User")!;
            MusicTypes = _context.Music_Types.ToList();
            Songs = _context.Songs.Include(s => s.Music_Type).ToList();
        }

        public IActionResult OnPost()
        {
            User = HttpContext.Session.GetObject<User>("User")!;
            switch (method)
            {
                case 1:
                    _context.Liked_Songs.Add(new Liked_Song { Id_Song = id, Id_User = User.Id });
                    break;
                case 2:
                    Liked_Song song = _context.Liked_Songs.Where(ls => ls.Id_Song == id && ls.Id_User == User.Id).First();
                    _context.Liked_Songs.Remove(song);
                    break;
            }
            _context.SaveChanges();
            return RedirectToPage();
        }

        public bool IsSongLiked(int id)
        {
            return _context.Liked_Songs.Any(ls => ls.Id_Song == id && ls.Id_User == HttpContext.Session.GetObject<User>("User")!.Id);
        }
    }
}
