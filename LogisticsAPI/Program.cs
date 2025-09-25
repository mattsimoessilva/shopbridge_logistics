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

var builder = WebApplication.CreateBuilder(args);

// Adding EF Core with Sqlite.
builder.Services.AddDbContext<LogisticsAppDbContext>(options =>
    options.UseSqlite("Data Source=logisticsapi.db"));

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
builder.Services.AddHttpClient<IOrderServiceGateway, OrderServiceGateway>(client =>
{
    client.BaseAddress = new Uri("http://localhost:3000/");
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