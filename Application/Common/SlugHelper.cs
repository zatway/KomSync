using System.Globalization;
using System.Text;

namespace Application.Common;

public static class SlugHelper
{
    public static string FromTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return "article";
        var sb = new StringBuilder();
        foreach (var c in title.Trim().ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(c))
                sb.Append(c);
            else if (char.IsWhiteSpace(c) || c is '-' or '_')
                sb.Append('-');
        }
        var s = sb.ToString().Trim('-');
        while (s.Contains("--", StringComparison.Ordinal))
            s = s.Replace("--", "-", StringComparison.Ordinal);
        return string.IsNullOrEmpty(s) ? "article" : s[..Math.Min(s.Length, 200)];
    }
}
