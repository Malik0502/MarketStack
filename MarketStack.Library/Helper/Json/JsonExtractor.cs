using System.Text.Json;

namespace MarketStack.Library.Helper.Json;

public static class JsonExtractor
{
    public static async Task<string?> ExtractResponseAsJsonAsync(HttpResponseMessage? response)
    {
        if (response == null)
            return null;

        var json = await response.Content.ReadAsStringAsync();

        return json;
    }

    public static T? DeserializeJson<T>(string json) where T : class
    {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var result = JsonSerializer.Deserialize<T>(json, serializeOptions);

        return result;
    }
}