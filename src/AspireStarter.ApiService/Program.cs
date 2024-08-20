using System.Net;
using Dapr.Client;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddDaprClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapPost("/sendmessages", async (
    [FromServices]DaprClient daprClient,
    [FromBody]string[] recipients,
    CancellationToken cancellationToken = default) => 
{
    var results = new List<SmsResult>();

    var messageBody = "welcome" switch
    {
        "welcome" => "Welcome to my app.",
        "leave" => "Goodbye!",
        _ => "Hi there!"
    };

    var sender = "+18001112222";

    foreach (var recipient in recipients)
    {
        var message = new SmsMessage(sender, recipient, messageBody);
        var result = await daprClient.InvokeMethodAsync<SmsMessage, SmsResult>(
            HttpMethod.Post,
            "sender",
            "send",
            message,
            cancellationToken);

        results.Add(result);
    }

    return Results.Ok(results);
});

app.MapDefaultEndpoints();

app.Run();

public record SmsMessage(string From, string To, string Body);
public record SmsResult(string From, string To, string Body, DateTime SentAt);
