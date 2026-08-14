using zadanie_kamsoft.DTOs;

namespace zadanie_kamsoft.Parsers;

public class CsvContentParser : IContentParser
{
    public content_type SupportedType => content_type.CSV;

    public ParseResponce Parse(String rawContent)
    {
        try
        {
            var lines = rawContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return new ParseResponce(false, 0, null, "Plik CSV jest pusty.");
            }
            var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
            var rowsList = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',').Select(v => v.Trim()).ToArray();
                var rowDict = new Dictionary<string, string>();

                for (int j = 0; j < headers.Length; j++)
                {
                    rowDict[headers[j]] = j < values.Length ? values[j] : string.Empty;
                }

                rowsList.Add(rowDict);
            }

            return new ParseResponce(
                Success: true,
                Processed_count: rowsList.Count,
                Data: rowsList
            );

        }
        catch (Exception ex)
        {
            return new ParseResponce
            (
                Success:false,
                Processed_count: 0,
                Data: null,
                ErrorMessege: $"Błąd podczas przetwarzania CSV: {ex.Message}"
            );
        }
    }
}