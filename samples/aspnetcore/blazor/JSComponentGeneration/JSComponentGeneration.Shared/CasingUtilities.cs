using System.Text.RegularExpressions;

namespace JSComponentGeneration.Shared
{
    public static class CasingUtilities
    {
        public static string PascalToCamelCase(string s)
            => char.ToLowerInvariant(s[0]) + s.Substring(1);

        public static string ToKebabCase(string s)
            => Regex.Replace(s, "[A-Z]+(?![a-z])|[A-Z]", EvaluateKebabCaseMatch);

        private static string EvaluateKebabCaseMatch(Match match)
            => (match.Index > 0 ? "-" : string.Empty) + match.Value.ToLowerInvariant();
    }
}
