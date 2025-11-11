using EasyNetQ;
using Monitoring.Shared;
using NewsletterService.Api.Messaging;
using NewsletterService.Api.Services;
using NewsletterService.Application.Interfaces;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.AddCentralMonitoringService();
builder.Services.AddHostedService<NewsletterSubscribeQueues>();
builder.Services.AddHostedService<NewsletterSubscriptionWelcomeMailQueues>();
builder.Services.AddSingleton<IBus>(_ =>
    RabbitHutch.CreateBus("host=rabbitmq;username=admin;password=happyheadlines", s => s.EnableSystemTextJson()));

builder.Services.AddScoped<IEmailSenderClient, EmailSenderClient>();

builder.Services.AddHttpClient("SubscriberService", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["Services:SubscriberApi"]!);
});

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
