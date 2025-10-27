using EasyNetQ;
using Monitoring.Shared;
using NewsletterService.Api.Messaging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.AddCentralMonitoringService();
builder.Services.AddHostedService<NewsletterSubscribeQueues>();
builder.Services.AddSingleton<IBus>(_ =>
    RabbitHutch.CreateBus("host=rabbitmq;username=admin;password=happyheadlines", s => s.EnableSystemTextJson()));

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
