using Dapr.Client;

namespace AspireStarter.Web;

public class SmsClient(DaprClient daprClient)
{
    public async Task<SmsResult[]> CreateMessagesAsync(string type, string[] recipients, CancellationToken cancellationToken = default)
    {
        var results = await daprClient.InvokeMethodAsync<string[], List<SmsResult>>(
            HttpMethod.Post,
            "apiservice",
            $"sendmessages",
            recipients,
            cancellationToken);        

        return results?.ToArray() ?? [];
    }
}

public record SmsMessage(string From, string To, string Body);
public record SmsResult(string From, string To, string Body, DateTime SentAt);