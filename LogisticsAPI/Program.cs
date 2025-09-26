using LogisticsAPI.Data;
using LogisticsAPI.Gateways;
using LogisticsAPI.Gateways.Interfaces;
using LogisticsAPI.Mappers;
using LogisticsAPI.Models.Profiles;
using LogisticsAPI.Repositories;
using LogisticsAPI.Repositories.Interfaces;
using LogisticsAPI.Services;
using LogisticsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetEnv;

// --- Load .env if it exists ---
Env.Load(); 

var builder = WebApplication.CreateBuilder(args);

// --- Database connection ---
var dbPath = Environment.GetEnvironmentVariable("LOGISTICS_DB_PATH")
             ?? "Storage/database.db"; 
builder.Services.AddDbContext<LogisticsAppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));


// Adding AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ShipmentProfile>();
    cfg.AddProfile<AvailabilityProfile>();
});

// Adding Repositories
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();

// Adding Gateways
builder.Services.AddHttpClient<IAddressValidationGateway, ViaCepGateway>(client =>
{
    client.BaseAddress = new Uri("https://viacep.com.br/"); 
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Adding Services
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IShippingService, ShippingService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ShopBridge - Logistics Service",
        Version = "v1",
        Description = "A RESTful service for managing shipments and verifying shipping availability within the ShopBridge platform.\r\nIt provides endpoints to create, update, track, and remove shipments, as well as to validate addresses and confirm serviceable destinations.",
        Contact = new OpenApiContact
        {
            Name = "@mattsimoessilva",
            Email = "matheussimoesdasilva@outlook.com"
        }
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// --- Apply migrations & seed database ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LogisticsAppDbContext>();
    db.Database.Migrate();
    DbInitializer.Initialize(db);
}

// --- Middleware & Swagger ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");


app.UseAuthorization();
app.MapControllers();

// --- Run using .env port or default ---
var port = Environment.GetEnvironmentVariable("LOGISTICS_SERVICE_PORT") ?? "8000";
app.Urls.Add($"http://*:{port}");
app.Run();