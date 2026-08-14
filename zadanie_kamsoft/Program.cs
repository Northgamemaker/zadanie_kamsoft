using zadanie_kamsoft.DTOs;
using zadanie_kamsoft.Parsers;
using zadanie_kamsoft.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rejestracja naszych serwisów w kontenerze Dependency Injection (DI)
builder.Services.AddSingleton<IBase64Decoder, Base64Decoder>();
builder.Services.AddScoped<IContentParser, CsvContentParser>();
builder.Services.AddScoped<IContentParser, InternalJsonContentParser>();

var app = builder.Build();

// Włączenie Swaggera w trybie deweloperskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// GLÓWNY ENDPOINT: POST /api/v1/parse-content
app.MapPost("/api/v1/parse-content", (
    ParseRequest request,
    IBase64Decoder base64Decoder,
    IEnumerable<IContentParser> parsers) =>
{
    // 1. Wyszukanie odpowiedniego parsera na podstawie typu z żądania (CSV / INTERNAL_JSON)
    var parser = parsers.FirstOrDefault(p => p.SupportedType == request.Type);
    if (parser is null)
    {
        return Results.BadRequest(new ParseResponce(
            Success: false,
            Processed_count: 0,
            Data: null,
            ErrorMessege: $"Typ '{request.Type}' nie jest obecnie obsługiwany."
        ));
    }

    // 2. Dekodowanie zawartości z Base64
    if (!base64Decoder.TryDecode(request.Content, out string decodedText, out string? errorMessage))
    {
        return Results.BadRequest(new ParseResponce(
            Success: false,
            Processed_count: 0,
            Data: null,
            ErrorMessege: errorMessage
        ));
    }

    // 3. Parsowanie odszyfrowanych danych
    var result = parser.Parse(decodedText);

    // 4. Jeśli wewnątrz parsera wystąpił błąd -> zwracamy HTTP 400 Bad Request
    if (!result.Success)
    {
        return Results.BadRequest(result);
    }

    // 5. Sukces -> zwracamy HTTP 200 OK z wynikiem
    return Results.Ok(result);
})
.WithName("ParseContent")
.WithOpenApi();

app.Run();