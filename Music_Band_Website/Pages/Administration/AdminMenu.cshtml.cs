using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music_Band_Website.Model;

namespace Music_Band_Website.Pages.Administration
{
    public class AdminMenuModel : PageModel
    {
        public User User { get; set; }

        public void OnGet()
        {
            User = HttpContext.Session.GetObject<User>("User")!;
        }
    }
}
