using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Decorators;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PizzaPlace.Helpers;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace PizzaPlace.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration config)
        {


            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaPlace", Version = "v1" });
            });

            services.AddControllers();
            services.AddScoped<IGuestRepository, GuestRepository>();

            services.AddScoped<IGuestOrderRepository, GuestOrderRepository>();
            services.Decorate<IGuestOrderRepository, CachingGuestOrderRepository>();


            services.AddScoped<IDevelopmentRepository, DevelopmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddSignalR();

            services.AddAutoMapper(typeof(MappingProfiles));

            // AUTH
            var key = Encoding.ASCII.GetBytes("very_secret_key!!!!!");

            services.AddAuthentication(options =>
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Employee", policy => policy.RequireClaim(ClaimTypes.Role, "CHEF", "WAITER"));
                options.AddPolicy("Manager", policy => policy.RequireClaim(ClaimTypes.Role, "MANAGER"));
            });

            services.AddMemoryCache();

            return services;
        }
    }
}
