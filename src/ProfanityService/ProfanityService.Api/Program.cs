using Microsoft.EntityFrameworkCore;
using ProfanityService.Application.Db;
using ProfanityService.Application.Interface;
using ProfanityService.Application.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IProfanityRepository, ProfanityRepository>();
builder.Services.AddDbContext<ProfanityDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
