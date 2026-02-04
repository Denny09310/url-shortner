using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointHandlers();
builder.Services.AddHybridCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapEndpointHandlers();

app.MapStaticAssets();
app.MapFallbackToFile("index.html");

app.Run();
