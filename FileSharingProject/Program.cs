using System.Globalization;
using FileSharingProject.Data;
using FileSharingProject.Helpers.Mail;
using FileSharingProject.ServicesManager;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//localiztion
builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportedCultures = new List<CultureInfo> {
        new CultureInfo("en-US"),
        new CultureInfo("ar"),
        new CultureInfo("fr")
    };
    opt.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    opt.SupportedCultures = supportedCultures;
    opt.SupportedUICultures = supportedCultures;
});
//************

builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetSection("ConStrWindows").Value);
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(op=> 
{
    op.SignIn.RequireConfirmedEmail = true;

})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
{
    o.TokenLifespan = TimeSpan.FromHours(3);
});


builder.Services.AddTransient<IMailHelper, MailHelper>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IUploadService, UploadService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
});
//authentication google&facebook
#region authentication google&facebook
//builder.Services.AddAuthentication().
//    AddFacebook( option =>
//    {
//        option.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
//        option.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
//    }).
//    AddGoogle(option =>
//    {
//        // var x = builder.Configuration.GetSection("Authentication:Google:ClientId").Value;
//        option.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
//        option.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
//    });
#endregion


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
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
//localization
IOptions<RequestLocalizationOptions>? locOption = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOption?.Value);

//**********************



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// migrations database
//using(var scop=app.Services.CreateScope())
//{
//    var db=scop.ServiceProvider.GetRequiredService<AppDbContext>();
//    db.Database.Migrate();// migrations
//     //here seed...

//}
//*********************
app.Run();

