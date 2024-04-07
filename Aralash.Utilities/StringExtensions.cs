using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Aralash.Utilities;

public static class StringExtensions
{
    public static bool IsNotEmpty([NotNullWhen(true)] this string? value) => !string.IsNullOrEmpty(value);

    public static bool IsEmpty([NotNullWhen(false)] this string? value) => string.IsNullOrEmpty(value);
    
    public static string FormatStackTrace(this string stackTrace) => 
        Regex.Replace(stackTrace, @"\r\n|\n|\r", Environment.NewLine);
}