namespace Bll.Common.Extension;

public static class StringUtil
{
    public static string StripBlanks(this string input)
    {
        var firstNonWhiteSpace = FirstNonWhiteSpace(input);
        var reversedInput = string.Join("", input.Reverse().ToList());
        var lastNonWhiteSpace = FirstNonWhiteSpace(reversedInput);
        
        return input.Substring(firstNonWhiteSpace, lastNonWhiteSpace + 1);
    }

    private static int FirstNonWhiteSpace(string input)
    {
        try
        {
            return input.First(c => !char.IsWhiteSpace(c));
        }
        catch (ArgumentNullException e)
        {
            return input.Length;
        }
    }
}