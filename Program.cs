using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Milestone.Models;
using Milestone.Services;
using Microsoft.Extensions.DependencyInjection;
using Milestone.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMilestoneLogger, NLogLogger>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();