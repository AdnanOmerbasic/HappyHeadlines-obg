using CommentService.Application.Db;
using CommentService.Application.Interface;
using CommentService.Application.Repository;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddDbContext<CommentDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CommentDb")));
builder.Services.AddHttpClient("ProfanityCheck", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["Services:ProfanityApi"]!);
});

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
