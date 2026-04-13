using Microsoft.EntityFrameworkCore;
using WebApplicationAPP.Data;
using WebApplicationAPP.Repositories;
using WebApplicationAPP.Bussines;

var builder = WebApplication.CreateBuilder(args);

// 🔥 MYSQL (CORRECTO)
var connectionString = builder.Configuration.GetConnectionString("MysqlConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// 🔥 INYECCIONES
builder.Services.AddScoped<ISinpeRepository, SinpeRepository>();
builder.Services.AddScoped<SinpeBusiness>();
builder.Services.AddScoped<IBitacoraService, BitacoraService>(); // 👈 ESTA FALTABA

builder.Services.AddControllers();

// 🔥 SWAGGER (para que veas el API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔥 SWAGGER UI
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();