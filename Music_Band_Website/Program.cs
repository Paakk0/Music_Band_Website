using Microsoft.EntityFrameworkCore;
using Music_Band_Website.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//SESSION
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//DATABASE
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Music_BandDB;Trusted_Connection=True;TrustServerCertificate=True;");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();
app.MapFallbackToPage("/Main");


app.Run();
