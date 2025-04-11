using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Music_Band_Website.Model;

namespace Music_Band_Website.Pages
{
    public class EventsModel : PageModel
    {
        private readonly ApplicationDBContext _context;
        public List<Event> Events { get; set; }

        public EventsModel(ApplicationDBContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Events = _context.Events.ToList();
        }
    }
}
