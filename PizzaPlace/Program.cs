using PizzaPlace;
using PizzaPlace.Extensions;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = "sk_test_51MVytIFqBUpBtecpwWeBDmvkFs3nGbEaeDS1qZYxWVYpmCPblHxCojWT5vpoIuI8qgN2wno0yLgBHJ9shXBj4qRn00gkQdko8n";

builder.Services.AddMyServices(builder.Configuration);

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
