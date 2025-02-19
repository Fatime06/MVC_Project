using Juan_Mvc_Project.Data;
using Juan_Mvc_Project.Models;
using Juan_Mvc_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<JuanDbContext>(options =>
{
	options.UseSqlServer(config.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
	opt.Password.RequireDigit = true;
	opt.Password.RequireLowercase = true;
	opt.Password.RequireUppercase = true;
	opt.Password.RequireNonAlphanumeric = true;
	opt.Password.RequiredLength = 6;
	opt.User.RequireUniqueEmail = true;

	opt.Lockout.MaxFailedAccessAttempts = 3;
	opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
	opt.Lockout.AllowedForNewUsers = true;
}).AddEntityFrameworkStores<JuanDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication();
	//.AddFacebook(options =>
	//{
	//	options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
	//	options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
	//});

builder.Services.AddScoped<EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
	  );

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();