using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TextAnalyzer.Api.Configuration;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SentimentResult
{
    Positive,
    Negative,
    Neutral
}

public static class SentimentConfiguration
{
    public static ChatOptions GetSentimentChatOptions()
    {
        var schema = JsonSerializer.Serialize(new
        {
            type = "object",
            properties = new
            {
                sentiment = new
                {
                    type = "string",
                    @enum = new[] { "Positive", "Neutral", "Negative" }
                }
            },
            required = new[] { "sentiment" }
        });

        return new ChatOptions
        {
            ResponseFormat = ChatResponseFormat.Json,
            AdditionalProperties = new AdditionalPropertiesDictionary { ["json_schema"] = schema }
        };
    }
}
