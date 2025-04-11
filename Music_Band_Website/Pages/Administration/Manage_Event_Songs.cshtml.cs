using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;

namespace Music_Band_Website.Pages.Administration
{
    public class Manage_Event_SongsModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public Event Event { get; set; }
        public List<Song> EventSongs { get; set; } = new List<Song>();
        public List<Song> AllSongs { get; set; } = new List<Song>();

        [BindProperty]
        public int SelectedSongId { get; set; }
        [BindProperty]
        public int EventId { get; set; }

        public Manage_Event_SongsModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Event = await _context.Events.FindAsync(id);
            if (Event == null)
            {
                TempData["Error"] = "Event not found!";
                return RedirectToPage("Manage_Events");
            }

            EventSongs = await _context.Event_Playlists
                .Where(ep => ep.Id_Event == id)
                .Include(ep => ep.Song)
                .ThenInclude(s => s.Music_Type)
                .Select(ep => ep.Song)
                .ToListAsync();

            AllSongs = await _context.Songs.Include(s => s.Music_Type).ToListAsync();

            return Page();
        }


        public async Task<IActionResult> OnPostAddSongAsync()
        {
            if (SelectedSongId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid song.");
                return Page();
            }

            var eventPlaylist = new Event_Playlist
            {
                Id_Event = EventId,
                Id_Song = SelectedSongId
            };

            _context.Event_Playlists.Add(eventPlaylist);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Song added successfully to the event!";
            return RedirectToPage(new { id = EventId });
        }

        public async Task<IActionResult> OnPostRemoveSongAsync(int songId)
        {
            var eventPlaylist = await _context.Event_Playlists
                .Where(ep => ep.Id_Event == EventId && ep.Id_Song == songId)
                .FirstOrDefaultAsync();

            if (eventPlaylist == null)
            {
                TempData["Message"] = "Song not found in this event!";
                return RedirectToPage(new { id = EventId });
            }

            _context.Event_Playlists.Remove(eventPlaylist);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Song removed successfully from the event!";
            return RedirectToPage(new { id = EventId });
        }
    }
}
