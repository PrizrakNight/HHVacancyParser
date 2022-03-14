namespace HHVacancyParser.Core.Extensions
{
    public static class StringEx
    {
        public static string RemoveUnicodeSpaces(this string @string)
        {
            return @string
                .Replace("\u00A0", string.Empty)
                .Replace("\u202F", string.Empty);
        }

        public static string RemoveMnemonics(this string @string)
        {
            return @string.Replace("&nbsp;", string.Empty);
        }
    }
}
