using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PizzaPlace;
using PizzaPlace.Helpers;
using Stripe;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = "sk_test_51MVytIFqBUpBtecpwWeBDmvkFs3nGbEaeDS1qZYxWVYpmCPblHxCojWT5vpoIuI8qgN2wno0yLgBHJ9shXBj4qRn00gkQdko8n";

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
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddSignalR();

builder.Services.AddAutoMapper(typeof(MappingProfiles));

// AUTH
var key = Encoding.ASCII.GetBytes("very_secret_key!!!!!"); 

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Employee", policy => policy.RequireClaim(ClaimTypes.Role, "CHEF", "WAITER"));
    options.AddPolicy("Manager", policy => policy.RequireClaim(ClaimTypes.Role, "MANAGER"));
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaPlace V1");
    });
}

app.UseRouting();

app.UseCors(x => x
           .AllowAnyMethod()
           .AllowAnyHeader()
           .SetIsOriginAllowed(origin => true)
           .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MyHub>("/myhub");

app.MapControllers();

app.Run();
