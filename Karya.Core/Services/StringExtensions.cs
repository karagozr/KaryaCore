namespace Karya.Core.Services;

public static class StringExtensions
{
    public static string FirstCharToLowerCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char firstChar = input[0];
        if (firstChar < 'A' || firstChar > 'Z')
            return input; 

        return string.Create(input.Length, input, (span, str) =>
        {
            str.CopyTo(span);
            span[0] = (char)(str[0] + 32);
        });
    } 
}

