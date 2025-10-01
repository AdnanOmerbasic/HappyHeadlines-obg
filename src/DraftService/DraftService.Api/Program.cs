using DraftService.Application.Db;
using DraftService.Application.Interfaces;
using DraftService.Application.Repository;
using Logging.Shared.Logging;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LoggingService.Logging();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDraftRepository, DraftRepository>();
builder.Services.AddDbContext<DraftDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DraftDb")));

builder.Host.UseSerilog();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
