using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Bussines;
using WebApplicationAPP.Data;
using WebApplicationAPP.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("MysqlConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("MysqlConnection")
        )
    );
});

// Clase;
//builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
//builder.Services.AddScoped<PersonaBussiness>();
//builder.Services.AddControllersWithViews();
// Repositories
//builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
//builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
// Business
//builder.Services.AddScoped<ClienteBusiness>();
//builder.Services.AddScoped<InventarioBusiness>();



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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
