using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;
using WebApplicationAPP.Repositories;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("MysqlConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("MysqlConnection")
        )
    );
});

// Repositories
builder.Services.AddScoped<IComercioRepository, ComercioRepository>();

// Business
builder.Services.AddScoped<ComercioBusiness>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();