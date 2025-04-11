using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Music_Band_Website.Model;
using Restaurant.Model;

namespace Music_Band_Website.Pages;

public class MainModel : PageModel
{
    private readonly ILogger<MainModel> _logger;
    private readonly ApplicationDBContext _context;
    public User User { get; set; }
    public Song LatestSong { get; set; }
    public Event NextConcert { get; set; }

    public MainModel(ILogger<MainModel> logger, ApplicationDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public string email { get; set; }

    public void OnGet()
    {
        User = HttpContext.Session.GetObject<User>("User")!;

        LatestSong = _context.Songs
            .OrderByDescending(s => s.Id)
            .First();

        if (!_context.Events.IsNullOrEmpty())
        {
            NextConcert = _context.Events
                .Where(c => c.DateTime.CompareTo(DateTime.Now) > 0)
                .OrderBy(c => c.DateTime)
                .FirstOrDefault()!;
        }
    }

    public IActionResult OnPost()
    {
        if (_context.Subscriptions.Any(s => s.Email.Equals(email)))
        {
            TempData["Subscription_Error"] = "Вече имате абонамент на този имейл адрес!";
            return RedirectToPage();
        }
        _context.Subscriptions.Add(new Subscription() { Email = email });
        _context.SaveChanges();
        _ = Mailer.SendMailAsync(new User() { Email = email }, "Subscription");
        TempData["Subscription_Success"] = "Вие успешно се абонирахте за нашия сайт! " +
            "Очаквайте актуални новини, специални оферти и ексклузивно съдържание директно във вашата пощенска кутия.";
        return RedirectToPage();
    }
}