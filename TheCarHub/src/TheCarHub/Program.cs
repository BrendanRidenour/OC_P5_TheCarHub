using FluentValidation;
using FluentValidation.AspNetCore;
using TheCarHub;
using TheCarHub.Controllers;
using Cookies = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor()
    .AddTransient<ISystemClock, SystemClock>()
    .AddTransient<IValidator<CarPoco>, CarPocoValidator>()
    .AddSingleton(new SecureLoginCredentials(
        username: builder.Configuration["Authentication:Username"],
        password: builder.Configuration["Authentication:Password"]))
    .AddTransient<IAuthenticationService, CookieAuthenticationService>()
    .AddSingleton<ICarRepository>(new AzureCarRepository(
        builder.Configuration["Azure:StorageAccountConnectionString"]))
    .AddTransient<IDealershipService, DealershipService>()
    .AddTransient<IAdminService, AdminService>();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = Cookies.AuthenticationScheme;
    auth.DefaultChallengeScheme = Cookies.AuthenticationScheme;
}).AddCookie(Cookies.AuthenticationScheme, cookie =>
{
    cookie.LoginPath = AdminController.Routes.Login;
});

builder.Services.AddControllersWithViews()
    .AddFluentValidation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(MainController.Routes.Error);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();