using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Souq.Data;
using Souq.Models;
using Souq.StartUp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.RegisterCustomServises();
builder.Services.RegisterSettings(builder.Configuration);

builder.Services.ConfigureIdentity();

builder.Services.RegisterDbContext(builder.Configuration);





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

app.UseStatusCodePagesWithRedirects("/Errors/{0}");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// To Seed {PaymentMethods, user, roles(Owners, Admins, Sellers, ShippingStaff)}
using (IServiceScope scope = app.Services.CreateScope())
{
    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    _context.Database.Migrate();

    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    SeedData.SeedDefaultDataAsync(_context, _roleManager, _userManager).Wait();
}

app.Run();
