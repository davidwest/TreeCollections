
namespace TreeCollections
{
    internal static class StringUtilityExtensions
    {
        public static string Wrap(this string originalText, string prefix, string postfix)
        {
            return prefix + originalText + postfix;
        }

        public static string Wrap(this string originalText, string wrapper)
        {
            return Wrap(originalText, wrapper, wrapper);
        }

        public static string WrapSingleQuotes(this string originalText)
        {
            return Wrap(originalText, "'");
        }

        public static string WrapDoubleQuotes(this string originalText)
        {
            return Wrap(originalText, @"""");
        }

        public static string WrapParentheses(this string originalText)
        {
            return Wrap(originalText, "(", ")");
        }

        public static string WrapSquareBrackets(this string originalText)
        {
            return Wrap(originalText, "[", "]");
        }

        public static string WrapCurlyBrackets(this string originalText)
        {
            return Wrap(originalText, "{", "}");
        }
    }
}
