using DraftService.Application.Db;
using DraftService.Application.Interfaces;
using DraftService.Application.Repository;
using Microsoft.EntityFrameworkCore;
using Monitoring.Shared;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddCentralMonitoringService();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddScoped<IDraftRepository, DraftRepository>();
builder.Services.AddDbContext<DraftDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DraftDb")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
