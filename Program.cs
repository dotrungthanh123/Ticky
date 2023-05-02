using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticky.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Ticky.Models;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TickyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectString") ?? throw new InvalidOperationException("Connection string 'TickyContext' not found.")));

builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("IsRetailer", o => o.RequireRole("3"));
    o.AddPolicy("IsAdmin", o => o.RequireRole("1"));
    o.AddPolicy("IsCustomer", o => o.RequireRole("2"));
});

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie(o =>
{
    o.AccessDeniedPath = "/Accounts/AccessDenied";
    o.LoginPath = "/Accounts/Login";
    //o.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
