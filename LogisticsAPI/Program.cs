using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using LogisticsAPI.Data;
using LogisticsAPI.Models.Profiles;
using LogisticsAPI.Repositories;
using LogisticsAPI.Repositories.Interfaces;
using LogisticsAPI.Services;
using LogisticsAPI.Services.Interfaces;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adding EF Core with Sqlite.
builder.Services.AddDbContext<LogisticsAppDbContext>(options =>
    options.UseSqlite("Data Source=logisticsapi.db"));

// Adding AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ShipmentProfile>();
});

// Adding Repositories
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();

// Adding Services
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
        Title = "Logistics API",
        Version = "v1",
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
    var db = scope.ServiceProvider.GetRequiredService<LogisticsAppDbContext>();
    db.Database.Migrate();
}

// Initializing database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LogisticsAppDbContext>();
    DbInitializer.Initialize(context);
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