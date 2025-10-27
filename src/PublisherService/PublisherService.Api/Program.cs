using EasyNetQ;
using Monitoring.Shared;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddCentralMonitoringService();
builder.Services.AddSingleton<IBus>(_ =>
    RabbitHutch.CreateBus("host=rabbitmq;username=admin;password=happyheadlines", s => s.EnableSystemTextJson()));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
