using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music_Band_Website.Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Music_Band_Website.Pages.Profile
{
    public class Sign_UpModel : PageModel
    {
        private readonly ApplicationDBContext context;
        public Sign_UpModel(ApplicationDBContext _context)
        {
            context = _context;
        }

        [BindProperty]
        public new required User User { get; set; }
        [BindProperty]
        public required string repeatPass { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var invalidFields = ModelState
                    .Where(kvp => kvp.Value!.Errors.Count > 0)
                    .Select(kvp => kvp.Key);

                TempData["Error"] = "Invalid fields: " + string.Join(", ", invalidFields);
                return Page();
            }
            if (context.Users.Any(u => u.Email.Equals(User.Email)))
            {
                TempData["User.Email"] = "This email already exists!";
                return Page();
            }
            if (User.Password != repeatPass)
            {
                TempData["User.Password"] = TempData["repeatPass"] = "Passwords must match!";
                return Page();
            }

            User.Is_Admin = false;
            User.Registration_Date = DateTime.Now;
            User.Password = HashPassword(User.Password);
            context.Users.Add(User);
            await context.SaveChangesAsync();
            HttpContext.Session.SetObject("User", User);
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
