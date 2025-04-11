using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;

namespace Music_Band_Website.Pages.Administration
{
    public class Manage_UsersModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public List<User> Users { get; set; }

        public Manage_UsersModel(ApplicationDBContext context)
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

            Users = _context.Users.ToList();
            return Page();
        }

        public IActionResult OnPostToggleAdmin(int userId, bool isAdmin)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            user.Is_Admin = !user.Is_Admin;
            _context.SaveChanges();

            if (user.Id == HttpContext.Session.GetObject<User>("User").Id)
            {
                HttpContext.Session.SetObject("User", user);
            }

            TempData["Message"] = $"User {user.Id} {(user.Is_Admin ? "is now an Admin" : "is no longer an Admin")}.";
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Message"] = $"User {user.Id} deleted.";
            return RedirectToPage();
        }
    }
}
