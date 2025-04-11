using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Music_Band_Website.Model;
using Org.BouncyCastle.Utilities;
using System;

namespace Music_Band_Website.Pages
{
    public class Song_PlayerModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public Song Song { get; set; }
        public User User { get; set; }
        public Song_PlayerModel(ApplicationDBContext context)
        {
            _context = context;
        }
        [BindProperty]
        public int method { get; set; }
        [BindProperty]
        public int id { get; set; }

        public IActionResult OnGet(int id)
        {
            User = HttpContext.Session.GetObject<User>("User")!;
            Song = _context.Songs.FirstOrDefault(s => s.Id == id)!;
            System.IO.File.WriteAllBytes("wwwroot/uploads/player.mp3", Song.Audio);
            return Page();
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
            return RedirectToPage(new { id = id });
        }

        public bool IsSongLiked(int id)
        {
            return _context.Liked_Songs.Any(ls => ls.Id_Song == id && ls.Id_User == HttpContext.Session.GetObject<User>("User")!.Id);
        }
    }
}
