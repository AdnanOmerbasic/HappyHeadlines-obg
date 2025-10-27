using CommentService.Application.Db;
using CommentService.Application.Interface;
using CommentService.Application.Repository;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Redis.Shared.Interfaces;
using Redis.Shared.Services;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var multiplexer = ConnectionMultiplexer.Connect(
    builder.Configuration.GetConnectionString("REDIS")!);


builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddScoped<IRedisCache, RedisCache>();

builder.Services.AddOpenApi();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddDbContext<CommentDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CommentDb")));
builder.Services.AddHttpClient("ProfanityCheck", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["Services:ProfanityApi"]!);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpMetrics();
app.MapMetrics();

app.UseAuthorization();

app.MapControllers();

app.Run();
