using System.Reflection;
using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Tracking.Core.Interfaces;
using Tracking.Core.Models;
using Tracking.Core.Services;
using Tracking.Core.Validators;
using Tracking.Infrastructure.Clients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Policy
    .Handle<KafkaException>()
    .RetryForeverAsync();

builder.Services.AddSingleton<IMessaging>(new KafkaMessaging("localhost:29092"));
builder.Services.AddScoped<IRedirectLinkService, RedirectLinkService>();
builder.Services.AddScoped<ITrackingService, TrackingService>();
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddFluentValidation(fv =>
    {
        fv.ImplicitlyValidateChildProperties = true;
        fv.ImplicitlyValidateRootCollectionElements = true;
        fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();