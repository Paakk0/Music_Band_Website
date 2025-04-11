using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music_Band_Website.Model;

namespace Music_Band_Website.Pages.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDBContext context;
        public new User? User { get; set; }
        public string? Role { get; set; }
        public ProfileModel(ApplicationDBContext _context)
        {
            context = _context;
        }
        public void OnGet()
        {
            User = HttpContext.Session.GetObject<User>("User");
            Role = User!.Is_Admin ? "Admin" : "User";
        }

        public IActionResult OnPost()
        {
            HttpContext.Session.Remove("User");
            return RedirectToPage("/Main");
        }
    }
}
