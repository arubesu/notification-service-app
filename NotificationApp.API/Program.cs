using System.Text.Json.Serialization;
using NotificationApp.Application.Extensions;
using NotificationApp.Application.Interfaces;
using NotificationApp.Application.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapPost("api/send-notification", async (NotificationRequest request, INotificationService notificationService) =>
{
    var result = await notificationService.SendAsync(request.Type, request.UserId, request.Message);

    if (result.IsError)
    {
        if (result.Errors.Any(errors => errors.Code == "Rate limit exceeded"))
        {
            return Results.Problem("Rate limit exceeded", statusCode: 429);
        }

        if (result.Errors.All(errors => errors.Type == ErrorOr.ErrorType.Validation))
        {
            return Results.BadRequest(new { Message = result.Errors });
        }

        return Results.StatusCode(500);
    }

    return Results.Ok(new { Message = "Notification sent successfully!" });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();