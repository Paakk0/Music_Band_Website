using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;
using Restaurant.Model;
using System.Collections.Generic;
using System.Linq;

namespace Music_Band_Website.Pages.Administration
{
    public class Manage_EventsModel : PageModel
    {
        private readonly ApplicationDBContext _context;

        public List<Event> Events { get; set; }

        [BindProperty]
        public Event NewEvent { get; set; }

        [BindProperty]
        public string location { get; set; }
        [BindProperty]
        public string city { get; set; }
        [BindProperty]
        public DateTime dateTime { get; set; }

        public Manage_EventsModel(ApplicationDBContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Events = _context.Events.ToList();
        }

        public async Task<IActionResult> OnPostAdd()
        {
            if (NewEvent != null)
            {
                _context.Events.Add(NewEvent);
                await _context.SaveChangesAsync();
                _ = NotifyUsersAsync();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(int id)
        {
            var eventToUpdate = _context.Events.Find(id);
            if (eventToUpdate != null)
            {
                eventToUpdate.Location = location;
                eventToUpdate.City = city;
                eventToUpdate.DateTime = dateTime;
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var eventToDelete = _context.Events.Find(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                _context.SaveChanges();
            }
            return RedirectToPage();
        }

        private async Task NotifyUsersAsync()
        {
            var tasks = _context.Subscriptions
                .Select(s => Mailer.SendMailAsync(new User { Email = s.Email }, "Event"))
                .ToList();

            await Task.WhenAll(tasks);
        }
    }
}
