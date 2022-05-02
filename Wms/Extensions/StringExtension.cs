using Wms.Helpers;

namespace Wms.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string src)
    {
        // This function does not transform abbreviations and acronyms.
        // That is to HTMLDoc, HTTPRequest will turn out to hTMLDoc, hTTPRequest

        // To have consistency, I regret that to say its probably better to:
        // 1. Always use snake-case! And if not, follow rule 2
        // 2. Always write acronyms and abbreviations in Pascal-case: HtmlDoc, Id, HttpRequest and not HTMLDoc, ID, HTTPRequest 
        // That way we get this: htmlDoc, id, httpRequest

        if (string.IsNullOrWhiteSpace(src))
            return string.Empty;

        return src.Length switch
        {
            1 => Char.ToLowerInvariant(src[0]).ToString(),
            _ => Char.ToLowerInvariant(src[0]) + src[1..]
        };

        // We probably can write the above into the following
        // but I'm opting for easy clarity.
        //return src switch
        //{
        //    null => string.Empty,
        //    _ => src.Length == 1 ? Char.ToLowerInvariant(src[0]).ToString() : Char.ToLowerInvariant(src[0]) + src[1..]
        //};
    }

    public static string Obfuscate(this string src)
    {
        return Creep.Encrypt(src);
    }
}
