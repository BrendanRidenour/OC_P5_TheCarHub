using FluentValidation;
using FluentValidation.AspNetCore;
using TheCarHub;
using TheCarHub.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ISystemClock, SystemClock>();
builder.Services.AddTransient<IDealershipService, DevDealershipService>();
builder.Services.AddTransient<IValidator<CarPoco>, CarPocoValidator>();
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