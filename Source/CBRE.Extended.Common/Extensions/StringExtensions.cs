namespace CBRE.Extended.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Split a string, but don't split within quoted values.
    /// </summary>
    /// <param name="Line">The line to split</param>
    /// <param name="SplitTest">Optional split test. Defaults to whitespace test.</param>
    /// <param name="QuoteChar">Optional quote character. Defaults to double quote.</param>
    /// <returns>An array of split values</returns>
    public static string[] SplitWithQuotes(this string Line, Func<char, bool>? SplitTest = null, char QuoteChar = '"')
    {
        if (SplitTest == null) SplitTest = Char.IsWhiteSpace;
        
        List<string> result = new List<string>();
        int index = 0;
        bool inQuote = false;
        
        for (int i = 0; i < Line.Length; i++)
        {
            char c = Line[i];
            bool isSplitter = SplitTest(c);
            if (isSplitter && index == i)
            {
                index = i + 1;
            }
            else if (c == QuoteChar)
            {
                inQuote = !inQuote;
            }
            else if (isSplitter && !inQuote)
            {
                result.Add(Line.Substring(index, i - index).Trim(QuoteChar));
                index = i + 1;
            }
            if (i != Line.Length - 1) continue;
            result.Add(Line.Substring(index, (i + 1) - index).Trim(QuoteChar));
        }
        return result.ToArray();
    }
        
    public static bool ToBool(this string Value)
    {
        string lowercaseValue = Value.ToLower();

        return lowercaseValue is "1" or "yes" or "true";
    }
}