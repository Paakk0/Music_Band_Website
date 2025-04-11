using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;

namespace Music_Band_Website.Pages
{
    public class Event_DetailsModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public User User { get; set; }
        public Event Event { get; set; }
        public List<Song> Event_Songs { get; set; }
        public int Event_Tickets { get; set; }

        public Event_DetailsModel(ApplicationDBContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int method { get; set; }

        [BindProperty]
        public int id { get; set; }

        public void OnGet(int id)
        {
            User = HttpContext.Session.GetObject<User>("User")!;
            Event = _context.Events.Find(id)!;

            Event_Songs = _context.Event_Playlists
                .Where(ep => ep.Id_Event == id)
                .Include(ep => ep.Song.Music_Type)
                .Select(ep => ep.Song!)
                .ToList();

            Event_Tickets = _context.Event_Tickets
                .Where(et => et.Id_Event == id)
                .Sum(et => et.TicketAmount);
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
                    var song = _context.Liked_Songs
                        .FirstOrDefault(ls => ls.Id_Song == id && ls.Id_User == User.Id);
                    if (song != null)
                    {
                        _context.Liked_Songs.Remove(song);
                    }
                    break;
            }
            _context.SaveChanges();
            return RedirectToPage("Event_Details", new { id });
        }

        public IActionResult OnPostBuyTickets()
        {
            Event = _context.Events.Find(Convert.ToInt32(Request.Form["id"]))!;
            User = HttpContext.Session.GetObject<User>("User")!;
            if (User == null)
                return RedirectToPage("Account/Sign_In");

            int ticketCount = Convert.ToInt32(Request.Form["TicketCount"]);

            var userTickets = new Event_Tickets
            {
                Id_Event = Event.Id,
                Id_User = User.Id,
                TicketAmount = ticketCount
            };
            _context.Event_Tickets.Add(userTickets);
            _context.SaveChanges();

            TempData["Message"] = $"Успешно закупихте {ticketCount} билет(а)!";
            return RedirectToPage("Event_Details", new { id = Event.Id });
        }

        public bool IsSongLiked(int id)
        {
            return _context.Liked_Songs.Any(ls => ls.Id_Song == id && ls.Id_User == HttpContext.Session.GetObject<User>("User")!.Id);
        }
    }
}
