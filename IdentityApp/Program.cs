using IdentityApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using IdentityApp.Models;
using IdentityApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add services to the container.

//builder.Services.AddTransient<IPasswordValidator<User>,CustomPasswordValidator>(serv => new CustomPasswordValidator(6));
//builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();

builder.Services.AddDbContext<IdentityTestDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityTestDB")));
builder.Services.AddIdentity<User, IdentityRole>(
    //opts => {
    //    opts.Password.RequiredLength = 5;   // ����������� �����
    //    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
    //    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    //    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    //    opts.Password.RequireDigit = false; // ��������� �� �����
    //}
    //opts => {
    //    opts.User.RequireUniqueEmail = true;    // ���������� email
    //    opts.User.AllowedUserNameCharacters = ".@abcdefghijklmnopqrstuvwxyz"; // ���������� �������
    //}
    )
    .AddEntityFrameworkStores<IdentityTestDbContext>();

    builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await RoleInitializer.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

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

app.UseAuthentication();    // ����������� ��������������
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
