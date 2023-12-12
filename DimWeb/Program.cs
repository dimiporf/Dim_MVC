using DimWeb.DataAccess.Data;
using DimWeb.DataAccess.Repository;
using DimWeb.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DimWeb.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using DimWeb.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Added after stackOverflow recommendation
builder.Services.AddMvc().AddRazorRuntimeCompilation();

// Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options=>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Stripe implementation
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

//facebook authentication logic
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "______This where you insert the app keys______";
    option.AppSecret = "______This where you insert the app keys______";
});



//add session for cart
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//add dbInitializer to services
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

//add Razor pages support
builder.Services.AddRazorPages();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//temporary way to bypass IEmail before implementation
builder.Services.AddScoped<IEmailSender, EmailSender>();

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

//Stripe segment
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

// Added after stackOverflow recommendation
app.MapControllers();   

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//session implementation
app.UseSession();

//invoke dbInitializer
SeedDatabase();


//add mapping for Razor pages
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


//DbInitializer

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
