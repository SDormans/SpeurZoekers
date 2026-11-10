using System.Text.RegularExpressions;

namespace UploadToolGui
{
    public static class MarkDown
    {
        public enum ListType
        {
            None,
            Unsorted,
            Numeric
        }

        public const string NewLine = "  \n", NewParagraph = "\n\n", 
            Header = "### ", Link = "[{0}]({1})", UnsortedList = "- ",
            ListPattern = @"^(?:(?<int>\d+)\. |(?<un>-) )",
            LinkPattern = @"\[(?<text>[^\]]*?)\]\((?<link>[^\)]*?)\)";
        public const char Strikeout = '~', ItalicBold = '*';
        public static readonly char[] EscapeChars = { '\\', '*', '~' }; // start with '\\'

        private const string NumList = ". ";

        public static string ContinueList(string precedingText)
        {
            string[] parts = precedingText.Split('\n');
            Match match = Regex.Match(parts[^1], ListPattern);

            if (match.Groups["int"].Success)
            {
                int index = int.Parse(match.Groups["int"].Value) + 1;
                return index + NumList;
            }
            else if (match.Groups["un"].Success)
            {
                return UnsortedList;
            }

            return string.Empty;
        }

        public static string ToggleList(string precedingText, ListType type, out int changeLenght)
        {
            string[] parts = precedingText.Split('\n');
            Match match = Regex.Match(parts[^1], ListPattern);
            string result;

            if (match.Groups["int"].Success)
            {
                changeLenght = match.Groups["int"].Value.Length * -1 - 2;
                parts[^1] = parts[^1].Substring(match.Groups["int"].Value.Length + 2);
                result = string.Join('\n', parts);
            }
            else if (match.Groups["un"].Success)
            {
                changeLenght = UnsortedList.Length * -1;
                parts[^1] = parts[^1].Substring(UnsortedList.Length);
                result = string.Join('\n', parts);
            }
            else if (type == ListType.Unsorted)
            {
                changeLenght = UnsortedList.Length;
                parts[^1] = UnsortedList + parts[^1];
                result = string.Join('\n', parts);
            }
            else if (type == ListType.Numeric)
            {
                string prefix = GetIndex(parts) + NumList;

                changeLenght = prefix.Length;
                parts[^1] = prefix + parts[^1];
                result = string.Join('\n', parts);
            }
            else
            {
                changeLenght = 0;
                result = precedingText;
            }

            return result;
        }

        private static int GetIndex(string[] parts)
        {
            int index = 1;

            if (parts.Length >= 3)
            {
                Match preMatch = Regex.Match(parts[^3], @"^(?:(?<int>\d+)\. |(?<un>-) )");

                if (preMatch.Groups["int"].Success)
                {
                    index = int.Parse(preMatch.Groups["int"].Value) + 1;
                }
            }

            return index;
        }
    }
}
