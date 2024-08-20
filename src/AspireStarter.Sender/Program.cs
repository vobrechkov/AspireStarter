using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler(options =>
{
    options.UseDeveloperExceptionPage();
});


app.UseHttpsRedirection();

app.MapPost("/send", ([FromBody]SmsMessage message) =>
{
    var result = new SmsResult(message.From, message.To, message.Body, DateTime.UtcNow);
    return Results.Ok(result);
})
.WithName("SendSms")
.WithOpenApi();

app.Run();

public record SmsMessage(string From, string To, string Body);
public record SmsResult(string From, string To, string Body, DateTime SentAt);
