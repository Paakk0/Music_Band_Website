using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;
using System.IO;

namespace Music_Band_Website.Pages.Administration
{
    public class Manage_MusicModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public List<Song> Songs { get; set; }
        public List<Music_Type> MusicTypes { get; set; }
        public Song EditingSong { get; set; }

        public Manage_MusicModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var loggedInUser = HttpContext.Session.GetObject<User>("User");
            if (loggedInUser == null || !loggedInUser.Is_Admin)
            {
                return RedirectToPage("/Account/Sign_In");
            }

            Songs = _context.Songs.Include(s => s.Music_Type).ToList();
            MusicTypes = _context.Music_Types.ToList();
            return Page();
        }

        public IActionResult OnPostAddType(string NewMusicType)
        {
            if (string.IsNullOrWhiteSpace(NewMusicType)) return Page();

            _context.Music_Types.Add(new Music_Type { Type = NewMusicType });
            _context.SaveChanges();
            TempData["Message"] = "Music Type added successfully!";
            return RedirectToPage();
        }

        public IActionResult OnPostAddSong(string Title, int Id_Type, string Description, IFormFile CoverImage, IFormFile AudioFile)
        {
            byte[] audioBytes;
            using (var memoryStream = new MemoryStream())
            {
                AudioFile.CopyTo(memoryStream);
                audioBytes = memoryStream.ToArray();
            }

            string imagePath = Path.Combine("wwwroot/uploads", CoverImage.FileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                CoverImage.CopyTo(stream);
            }

            string relativeImagePath = $"/uploads/{CoverImage.FileName}";

            var song = new Song
            {
                Title = Title,
                Id_Type = Id_Type,
                Description = Description,
                Cover_Image = relativeImagePath,
                Audio = audioBytes
            };

            _context.Songs.Add(song);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostUpdateType(int typeId, string UpdatedType)
        {
            var type = _context.Music_Types.Find(typeId);
            if (type == null) return NotFound();

            type.Type = UpdatedType;
            _context.SaveChanges();
            TempData["Message"] = "Music Type updated!";
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteType(int typeId)
        {
            var type = _context.Music_Types.Find(typeId);
            if (type == null) return NotFound();

            _context.Music_Types.Remove(type);
            _context.SaveChanges();
            TempData["Message"] = "Music Type deleted!";
            return RedirectToPage();
        }

        public IActionResult OnPostEditSong(int songId)
        {
            EditingSong = _context.Songs.Find(songId);
            Songs = _context.Songs.Include(s => s.Music_Type).ToList();
            MusicTypes = _context.Music_Types.ToList();
            return Page();
        }

        public IActionResult OnPostUpdateSong(int SongId, string Title, int Id_Type, string Description, IFormFile CoverImage, IFormFile AudioFile)
        {
            var song = _context.Songs.Find(SongId);
            if (song == null) return NotFound();

            song.Title = Title;
            song.Id_Type = Id_Type;
            song.Description = Description;

            if (CoverImage != null)
            {
                string filePath = "/uploads/" + CoverImage.FileName;
                using (var stream = new FileStream("wwwroot" + filePath, FileMode.Create))
                {
                    CoverImage.CopyTo(stream);
                }
                song.Cover_Image = filePath;
            }

            if (AudioFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    AudioFile.CopyTo(ms);
                    song.Audio = ms.ToArray();
                }
            }

            _context.SaveChanges();
            TempData["Message"] = "Song updated!";
            return RedirectToPage();
        }
    }
}
