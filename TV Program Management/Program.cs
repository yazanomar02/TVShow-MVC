using Domain.Models;
using Infrastructure.DbContexts;
using Infrastructure.Repositorys;
using Microsoft.AspNetCore.Authentication.Cookies;
using TV_Program_Management.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Register DbContext
builder.Services.AddDbContext<TVDbContext>(); //*


builder.Services.AddScoped<IRepository<User> ,UserRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<TVShow>, TvShowRepository>();
builder.Services.AddScoped<ITvShowRepository, TvShowRepository>();
builder.Services.AddScoped<IRepository<Attachment>, AttachmentRepository>();
builder.Services.AddScoped<IRepository<Language>, LanguageRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();


// Register Session
builder.Services.AddSession();
// Register HttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Register SessionStateRepository
builder.Services.AddTransient<IStateRepository, SessionStateRepository>();


// Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60); // عمر الكوكي
        option.LoginPath = "/Login"; // في حال لم يحقق سيوجه اليوزر الى صفحة تسجيل الدخول
        option.SlidingExpiration = true; // توليد كوكي جديد كل نصف الوقت
    });

var app = builder.Build();

app.UseSession();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
