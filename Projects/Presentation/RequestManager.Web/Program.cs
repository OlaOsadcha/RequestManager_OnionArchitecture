using Microsoft.AspNetCore.Identity;
using RequestManager.Core.Domain.Email;
using RequestManager.Core.Domain.Entities;
using RequestManager.Infrastructure;
using RequestManager.Infrastructure.Configurations.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);
string CURRENT_ASSEMBLY_NAME = "RequestManager.Web";

// Add services to the container.

builder.Services.AddAutoMapper(Assembly.Load(CURRENT_ASSEMBLY_NAME));
builder.Services.AddPersistanceRegistrationService(builder.Configuration);

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddMvc();
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddIdentity<RequestUser, RequestRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;

    //opts.SignIn.RequireConfirmedEmail = true;

    // User settings.
    opts.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "RequestManagerCookie";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromMinutes(15));

builder.Services.AddControllersWithViews();

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
app.UseSession();
app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();