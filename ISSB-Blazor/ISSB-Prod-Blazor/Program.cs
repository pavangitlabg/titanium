using System.Net;
using Syncfusion.Licensing;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using ISSB_Prod_Blazor;
using ISSB_Prod_Blazor.Helpers;
using ISSB_Prod_Blazor.Hubs;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 102400000;
});
builder.Services.AddSyncfusionBlazor();
builder.Services.AddSignalR();

//builder.Services.AddIdentityCore<IdentityUser>(options =>
//{
//    options.Password.RequireDigit = false;
//    options.Password.RequiredLength = 5;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.SignIn.RequireConfirmedEmail = false;
//});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSession(opts =>
{
    opts.Cookie.IsEssential = true; // make the session cookie Essential
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.AccessDeniedPath = "/Home/ErrorForbidden";
                 options.LoginPath = "/Login/Login";
             });


builder.Services.AddScoped<NavigationModelService>();
//services
builder.Services.AddHostedService<ImportService>();
builder.Services.AddHostedService<OpenExchangeService>();

builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<PortsService>(); 
builder.Services.AddScoped<TariffService>();

builder.Services.AddScoped<DataImportService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddScoped<UpsertService>();
builder.Services.AddScoped<ImportFileTypeService>();
builder.Services.AddSingleton<MessageService>();
//builder.Host.UseContentRoot(AppContext.BaseDirectory);

#if RELEASE
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.IPEndPoint.Port = 6060;
        listenOptions.IPEndPoint.Address = IPAddress.Loopback;

        listenOptions.KestrelServerOptions.Limits.MaxRequestBodySize = 1024 * 1024 * 500;//= 20000000000;
        listenOptions.KestrelServerOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
        listenOptions.KestrelServerOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
    });
});
#endif

var app = builder.Build();

//License required from Syncfusion 
SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NMaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWX5fdXRURGlYV0xwV0Q=");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization("en-GB");
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");


app.UseEndpoints(endpoints =>
{ 
    endpoints.MapControllers();
});


app.MapHub<MessageHub>("/hubs/messagehub");

app.Run();

