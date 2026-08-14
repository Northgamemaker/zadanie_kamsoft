using System.Text.Json.Serialization;

namespace zadanie_kamsoft.DTOs;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum content_type
    {
        CSV,
        INTERNAL_JSON
    }


