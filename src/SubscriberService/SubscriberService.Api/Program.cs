using EasyNetQ;
using FeatureHubSDK;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SubscriberService.Api.Middleware;
using SubscriberService.Application.Db;
using SubscriberService.Application.Interface;
using SubscriberService.Application.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var config = new EdgeFeatureHubConfig(builder.Configuration["Featurehub:Host"], builder.Configuration["Featurehub:ApiKey"]);
var fhClient = await config.NewContext().Build();

builder.Services.AddSingleton<IClientContext>(fhClient);

builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
builder.Services.AddDbContext<SubscriberDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SubscriberDb")));

builder.Services.AddSingleton<IBus>(_ =>
    RabbitHutch.CreateBus(builder.Configuration["RabbitMQ"], s => s.EnableSystemTextJson()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<SubscriberServiceMiddleware>();

app.MapControllers();

app.Run();
