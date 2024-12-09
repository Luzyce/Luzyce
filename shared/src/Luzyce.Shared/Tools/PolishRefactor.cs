using System.Text;

namespace Luzyce.Shared.Tools;

public static class PolishRefactor
{
    private readonly static Dictionary<char, char> polishCharMap = new()
    {
        {'ą', 'a'}, {'ć', 'c'}, {'ę', 'e'}, {'ł', 'l'}, {'ń', 'n'}, {'ó', 'o'}, {'ś', 's'}, {'ź', 'z'}, {'ż', 'z'},
        {'Ą', 'A'}, {'Ć', 'C'}, {'Ę', 'E'}, {'Ł', 'L'}, {'Ń', 'N'}, {'Ó', 'O'}, {'Ś', 'S'}, {'Ź', 'Z'}, {'Ż', 'Z'}
    };

    public static string? RemovePolishChars(string? input)
    {
        if (input is null)
        {
            return null;
        }
        
        var sb = new StringBuilder(input.Length);

        foreach (var c in input)
        {
            sb.Append(polishCharMap.GetValueOrDefault(c, c));
        }

        return sb.ToString();
    }
}
