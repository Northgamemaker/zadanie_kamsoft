using System.Text.Json;
using zadanie_kamsoft.DTOs;

namespace zadanie_kamsoft.Parsers;

public class InternalJsonContentParser : IContentParser
{
    public content_type SupportedType => content_type.INTERNAL_JSON;

    public ParseResponce Parse(string rawContent)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(rawContent);

            int count = 0;
            object parsedData;

            if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
            {
                count = jsonDoc.RootElement.GetArrayLength();
                parsedData = JsonSerializer.Deserialize<object[]>(rawContent)!;
            }
            else
            {
                count = 1;
                parsedData = JsonSerializer.Deserialize<object>(rawContent)!;
            }

            return new ParseResponce(
                Success: true,
                Processed_count: count,
                Data: parsedData
            );
        }
        catch (JsonException ex)
        {
            return new ParseResponce(
                Success: false,
                Processed_count: 0,
                Data: null,
                ErrorMessege: $"Błąd formatu JSON: {ex.Message}"
            );
        }
    }
}