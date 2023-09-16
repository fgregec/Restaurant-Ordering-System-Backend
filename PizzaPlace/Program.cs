using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaPlace.Helpers;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = "sk_test_51MVytIFqBUpBtecpwWeBDmvkFs3nGbEaeDS1qZYxWVYpmCPblHxCojWT5vpoIuI8qgN2wno0yLgBHJ9shXBj4qRn00gkQdko8n";

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options =>
options.UseSqlServer(connectionString));
    
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaPlace", Version = "v1" });
});

builder.Services.AddControllers();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IGuestOrderRepository, GuestOrderRepository>();
builder.Services.AddScoped<IDevelopmentRepository, DevelopmentRepository>();


builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddAutoMapper(typeof(MappingProfiles));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaPlace V1");
});

app.UseCors("corspolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
