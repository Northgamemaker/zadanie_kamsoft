using System.Text;
namespace zadanie_kamsoft.Services;
public class Base64Decoder : IBase64Decoder
{
    public bool TryDecode(string base64Content, out string decodedText, out string? errorMessage)
    {
        decodedText = string.Empty;
        errorMessage = null;
        if (string.IsNullOrWhiteSpace(base64Content))
        {
            errorMessage = "Zawartość Base64 nie może być pusta";
            return false;
        }
        try
        {
            byte[] Bytes = Convert.FromBase64String(base64Content);
            decodedText = Encoding.UTF8.GetString(Bytes);
            return true;
        }
        catch(FormatException)
        {
            errorMessage = "Podany ciąg znaków nie jest ciągiem Base 64";
            return false;
        }
    }
}