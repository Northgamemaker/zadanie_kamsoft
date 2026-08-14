namespace zadanie_kamsoft.Services;

public interface IBase64Decoder
{
    bool TryDecode(string Base64Content, out string decodedText, out string ErrorMessege);
}