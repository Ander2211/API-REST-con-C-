using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
// Swagger/OpenAPI types removed to avoid Microsoft.OpenApi assembly conflicts

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version")
    );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // e.g. v1, v2
    options.SubstituteApiVersionInUrl = true;
});

// Swagger generation removed to avoid runtime type conflicts with Microsoft.OpenApi

var app = builder.Build();

// Configure the HTTP request pipeline.
// Intercept requests to /swagger to avoid invoking Swagger middleware
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.CompleteAsync();
        return;
    }
    await next();
});
if (app.Environment.IsDevelopment())
{
    // Swagger middleware removed to avoid runtime errors when Swagger services
    // (e.g. AddSwaggerGen) are not registered or are incompatible.

    // MapOpenApi removed to avoid Microsoft.OpenApi runtime type conflicts
}

app.UseHttpsRedirection();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}






