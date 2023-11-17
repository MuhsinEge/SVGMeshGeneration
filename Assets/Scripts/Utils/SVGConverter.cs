using System.Text.RegularExpressions;
using System;
using UnityEngine;

public static class SVGConverter 
{
    private static readonly char[] PathSeparators = { 'M', 'L', 'Z', 'H', 'V', 'C', 'S', 'Q', 'T', 'A' };
    private static readonly string pattern = "path\\s+d=\"([^\"]*)\"";

    public static string[] GenerateCoordinates(TextAsset SvgText)
    {
        string pathAttribute = GetAttributeValue(SvgText.text, pattern);
        Debug.Log("Path Attribute: " + pathAttribute);

        string svgPath = RemoveQuotes(pathAttribute);
        Debug.Log("SVG Path: " + svgPath);

        svgPath = AddSeparatorPrefix(svgPath);
        Debug.Log("Modified SVG Path: " + svgPath);

        return GetCoordinates(svgPath);
    }

    private static string GetAttributeValue(string input, string pattern)
    {
        Regex regex = new Regex(pattern, RegexOptions.CultureInvariant);
        Match match = regex.Match(input);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    private static string RemoveQuotes(string input)
    {
        return input.Replace("\"", "");
    }

    private static string AddSeparatorPrefix(string input)
    {
        foreach (char separator in PathSeparators)
        {
            input = input.Replace(separator.ToString(), "#" + separator);
        }
        return input;
    }

    private static string[] GetCoordinates(string pathString)
    {
        string[] lines = pathString.Split('#');

        // Skip the empty first element if present
        if (string.IsNullOrEmpty(lines[0]))
        {
            lines = lines.RemoveAt(0);
        }

        foreach (string line in lines)
        {
            Debug.Log(line);
        }
        return lines;
    }

    public static T[] RemoveAt<T>(this T[] array, int index)
    {
        if (index < 0 || index >= array.Length)
        {
            return array;
        }

        T[] newArray = new T[array.Length - 1];
        Array.Copy(array, 0, newArray, 0, index);
        Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
        return newArray;
    }
}
