using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Music_Band_Website.Model;
using System.Security.Cryptography;
using System.Text;

namespace Music_Band_Website.Pages.Profile
{
    public class Sign_InModel : PageModel
    {
        private readonly ApplicationDBContext context;
        public Sign_InModel(ApplicationDBContext _context)
        {
            context = _context;
        }
        [BindProperty] public required string email { get; set; }
        [BindProperty] public required string password { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var invalidFields = ModelState
                    .Where(kvp => kvp.Value!.Errors.Count > 0)
                    .Select(kvp => kvp.Key);

                TempData["Error"] = "Invalid fields: " + string.Join(", ", invalidFields);
                return Page();
            }
            if (!context.Users.Any(u => u.Email.Equals(email)))
            {
                TempData["EmailError"] = "Could not find account with this email!";
                return Page();
            }
            var hashedCurrentPassword = HashPassword(password);
            if (!context.Users.Any(u => u.Password.Equals(hashedCurrentPassword)))
            {
                TempData["PasswordError"] = "Incorrect password!";
                return Page();
            }
            User user = context.Users.First(u => u.Email.Equals(email) && u.Password.Equals(hashedCurrentPassword));
            HttpContext.Session.SetObject("User", user);
            return RedirectToPage("/Main");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
