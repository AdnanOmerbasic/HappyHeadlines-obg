using ArticleService.Api.Messaging;
using ArticleService.Application.Db;
using ArticleService.Application.Interfaces;
using ArticleService.Application.Repository;
using EasyNetQ;
using Monitoring.Shared;
using Prometheus;
using Redis.Shared.Interfaces;
using Redis.Shared.Services;
using Scalar.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var multiplexer = ConnectionMultiplexer.Connect(
    builder.Configuration.GetConnectionString("REDIS")!);

// Add services to the container.
builder.AddCentralMonitoringService();

builder.Services.AddHostedService<ArticleSubscribeQueues>();
builder.Services.AddHostedService<ArticlesRespondRequests>();
builder.Services.AddHostedService<ArticlePrewarm>();

builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddScoped<IRedisCache, RedisCache>();

builder.Services.AddSingleton<IBus>(_ =>
    RabbitHutch.CreateBus("host=rabbitmq;username=admin;password=happyheadlines", s => s.EnableSystemTextJson()));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

//app.UseHttpsRedirection();

app.UseHttpMetrics();
app.MapMetrics();

app.UseAuthorization();

app.MapControllers();

app.Run();
