using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music_Band_Website.Model;
using System.Security.Cryptography;
using System.Text;

namespace Music_Band_Website.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDBContext context;

        public EditModel(ApplicationDBContext _context)
        {
            context = _context;
        }

        [BindProperty]
        public User User { get; set; }

        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmNewPassword { get; set; }

        public void OnGet()
        {
            User = HttpContext.Session.GetObject<User>("User")!;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(CurrentPassword))
            {
                if (!ModelState.IsValid)
                {
                    var invalidFields = ModelState
                        .Where(kvp => kvp.Value!.Errors.Count > 0)
                        .Select(kvp => kvp.Key);

                    TempData["Error"] = "Invalid fields: " + string.Join(", ", invalidFields);
                    return Page();
                }

                if (NewPassword != ConfirmNewPassword)
                {
                    TempData["Error"] = "New password and confirmation do not match.";
                    return Page();
                }

                var hashedCurrentPassword = HashPassword(CurrentPassword);
                if (User.Password != hashedCurrentPassword)
                {
                    TempData["Error"] = "Current password is incorrect.";
                    return Page();
                }

                var currUser = context.Users.Find(User.Id)!;
                User.Registration_Date = currUser.Registration_Date;

                var hashedNewPassword = HashPassword(NewPassword);
                User.Password = hashedNewPassword;

                context.Entry(currUser!).CurrentValues.SetValues(User);
                await context.SaveChangesAsync();

                TempData["Success"] = "Password changed successfully!";
                HttpContext.Session.Remove("User");
                return RedirectToPage("/Account/Sign_in");
            }
            else
            {
                ModelState.Remove("NewPassword");
                ModelState.Remove("CurrentPassword");
                ModelState.Remove("ConfirmNewPassword");

                if (!ModelState.IsValid)
                {
                    var invalidFields = ModelState
                        .Where(kvp => kvp.Value!.Errors.Count > 0)
                        .Select(kvp => kvp.Key);

                    TempData["Error"] = "Invalid fields: " + string.Join(", ", invalidFields);
                    return Page();
                }

                if (context.Users.Any(u => u.Id != User.Id && u.Email.Equals(User.Email)))
                {
                    TempData["Error"] = "This email already exists!";
                    return Page();
                }

                var currentUser = await context.Users.FindAsync(User.Id);
                User.Password = currentUser!.Password;
                User.Registration_Date = currentUser.Registration_Date;
                User.Is_Admin = currentUser.Is_Admin;
                if (currentUser != null)
                {
                    context.Entry(currentUser).CurrentValues.SetValues(User);
                    await context.SaveChangesAsync();
                    HttpContext.Session.SetObject("User", User);
                }

            }
            Thread.Sleep(1000);
            return RedirectToPage("/Account/Profile");
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
