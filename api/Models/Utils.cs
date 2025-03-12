using System.Text.RegularExpressions;

namespace API.Models.Utils;

public static partial class RegexUtils {
    public static int ExtractScore(string rating)
    {
        var match = MyRegex().Match(rating);
        if (match.Success) {
            return int.Parse(match.Value);
        }
        return 1;
    }

    // Response: {"character":[{"name":"Rachel Howard","id":22}{...}]}
    // Needs to extract 
    public static string[] ExtractCharacterIds(string response) {
        return [];
    }

    public static bool ExtractPass(string rating) { return rating.Contains("\"true\""); }
    public static string ExtractMood(string response) {
        try {
            var parts = response.Split("\"mood\": \"");
            if (parts.Length > 1) {
                return parts[1].Split("\"")[0];
            }
            return null;
        } catch {
            return null;
        }    
    }
    public static string ExtractMemory(string response) { 
        try {
            var parts = response.Split("\"memory\": \"");
            if (parts.Length > 1) {
                return parts[1].Split("\"")[0];
            }
            return null;
        } catch {
            return null;
        }
    }
    public static string CleanResponse(string response) { return MyRegex1().Replace(response, "").Trim(); }

    [GeneratedRegex("\\d+")]
    public static partial Regex MyRegex();

    [GeneratedRegex(@"\([A-Za-z\s\.\']+\):\s*")]
    private static partial Regex MyRegex1();
}
