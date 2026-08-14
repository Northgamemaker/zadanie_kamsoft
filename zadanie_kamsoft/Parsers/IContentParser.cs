using System.Text.Json;
using zadanie_kamsoft.DTOs;

namespace zadanie_kamsoft.Parsers;

public interface IContentParser
{
   
    content_type SupportedType { get; }

    
    ParseResponce Parse(string rawContent);
}
