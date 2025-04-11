using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music_Band_Website.Model;
using Restaurant.Model;
using System.Security.Cryptography;
using System.Text;

namespace Music_Band_Website.Pages.Account
{
    public class Retreive_PasswordModel : PageModel
    {
        private readonly ApplicationDBContext context;

        public Retreive_PasswordModel(ApplicationDBContext _context)
        {
            context = _context;
        }

        [BindProperty]
        public string? email { get; set; }
        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!context.Users.Any(u => u.Email.Equals(email)))
            {
                TempData["Error"] = "Не е намерен подобен имейл адрес!";
                return Page();
            }
            Random random = new Random();
            string newPassword = random.Next(10000000, 99999900).ToString();
            User user = context.Users.FirstOrDefault(u => u.Email.Equals(email))!;
            user.Password = newPassword;
            _ = Mailer.SendMailAsync(user, "Password Retreival");
            User newUser = user;
            newUser.Password = HashPassword(newPassword);
            context.Entry(user).CurrentValues.SetValues(newUser);
            context.SaveChanges();
            TempData["Success"] = "Новата ви парола беше изпратена на посочения имейл адрес!";
            return Page();
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
