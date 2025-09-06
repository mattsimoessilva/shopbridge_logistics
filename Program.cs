using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using LogisticsAPI.Data;
using LogisticsAPI.Repositories;
using LogisticsAPI.Repositories.Interfaces;
using LogisticsAPI.Services;
using LogisticsAPI.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adding EF Core with Sqlite.
builder.Services.AddDbContext<LogisticsDbContext>(options =>
    options.UseSqlite("Data Source=logistics.db"));

// Adding services.
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Logistics API",
        Description = "An ASP.NET Core Web API for managing logistics in the ShopBridge system.",
        Contact = new OpenApiContact
        {
            Name = "Matheus Simões",
            Email = "matheussimoesdasilva@outlook.com"
        }
    });
});

var app = builder.Build();

// Applying migrations.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LogisticsDbContext>();
    db.Database.Migrate();
}

// Configuring the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();