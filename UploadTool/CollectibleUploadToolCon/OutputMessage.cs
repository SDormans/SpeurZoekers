using System.Collections.Generic;
using System.Text;

namespace CollectibleUploadToolCon
{
    public enum ErrorLevel
    {
        Warning = Options.ShowWarnings,
        Error = Options.ShowErrors,
        SkippedFile = Options.ShowSkippedFiles,
        ReplacedItem = Options.ShowReplacedItems,
        SuccesfulItem = Options.ShowSuccesfulItems
    }

    public enum ErroneousFile
    {
        Csv,
        Image,
        Text
    }

    public class OutputMessage
    {
        public ErrorLevel Level { get; set; }
        public string Text { get; set; }
    }

    public static class OutputMessageExtensions
    {
        public static void AddSkippedItem(this IList<OutputMessage> list, string name)
        {
            list.Add(new OutputMessage
            {
                Level = ErrorLevel.SkippedFile,
                Text = "Overgeslagen item     ; " + name
            });
        }

        public static void AddReplacedItem(this IList<OutputMessage> list, string name)
        {
            list.Add(new OutputMessage
            {
                Level = ErrorLevel.ReplacedItem,
                Text = "Item overschreven     ; " + name
            });
        }

        public static void AddSuccessfulItem(this IList<OutputMessage> list, string name)
        {
            list.Add(new OutputMessage
            {
                Level = ErrorLevel.SuccesfulItem,
                Text = "Item uitgelezen       ; " + name
            });
        }

        public static void AddMessage(this IList<OutputMessage> list, ErrorLevel level, ErroneousFile file, string message)
        {
            list.Add(new OutputMessage
            {
                Level = level,
                Text = CreateMessage(level, file, message)
            });
        }

        public static void AddMessage(this IList<OutputMessage> list, ErrorLevel level, ErroneousFile file, string message, int row)
        {
            string upon = file == ErroneousFile.Csv ? " op rij " : " op regel ";

            list.Add(new OutputMessage
            {
                Level = level,
                Text = CreateMessage(level, file, message) + upon + (row + 1)
            });
        }
        
        public static void AddMessage(
            this IList<OutputMessage> list, 
            ErrorLevel level, 
            ErroneousFile file, 
            string message, 
            int row, 
            int column,
            string columnName)
        {
            list.Add(new OutputMessage
            {
                Level = level,
                Text = CreateMessage(level, file, message) + $" op [{GetLetter(column)}{row + 1}] ({columnName})"
            });
        }

        public static string GetLetter(int column)
        {
            if (column == 0)
            {
                return "A";
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 1; column / i > 0; i*=26)
            {
                builder.Insert(0, (char)(((column + 1) / i - 1) % 26 + 'A'));
            }

            return builder.ToString();
        }

        private static string CreateMessage(ErrorLevel level, ErroneousFile file, string message)
        {
            string msg = string.Empty;

            switch (level)
            {
                case ErrorLevel.Warning:
                    msg = "Waarschuwing ";
                    break;
                case ErrorLevel.Error:
                    msg = "Foutmelding  ";
                    break;
            }

            switch (file)
            {
                case ErroneousFile.Csv:
                    msg += "(lijst  ); ";
                    break;
                case ErroneousFile.Image:
                    msg += "(plaatje); ";
                    break;
                case ErroneousFile.Text:
                    msg += "(tekst  ); ";
                    break;
            }

            msg += message;
            return msg;
        }
    }
}
