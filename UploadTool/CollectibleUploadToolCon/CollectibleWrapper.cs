using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using Speurzoekers.Data.Entities.Collectible;
using Speurzoekers.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CollectibleUploadToolCon
{
    public class CollectibleWrapper : CollectibleType
    {
        private static readonly string escAnswer = Regex.Escape(AnswerMarkup);
        private static readonly int NameMaxLength =
            RepositoryHelper.GetColumnMaxStringLength<CollectibleTypeEntity>(x => x.Name),
            TagMaxLength = RepositoryHelper.GetColumnMaxStringLength<CollectibleTypeEntity>(x => x.TagName);

        public const char CsvSeparator = ';';
        public const bool AllowMultipleAnswers = false;
        public const int CsvColumnNameLine = 1, CsvHeaderLines = 2, ImageWidth = 250, ImageHeight = 250;
        public const CollectibleRarity DefaultRarity = CollectibleRarity.Common;
        public const string CsvCheckedValue = "x", ImageExtension = ".png", HeaderPrefix = "# ",
            QuestionHeader = "Vraag", AnswerMarkup = "**" /* bold */, UnorderedListMarkup = "- ",
            ChoiceGroup = "choice", AnswerStart0 = "answerStart0", AnswerStart1 = "answerStart1", AnswerEnd = "answerEnd";
        public static readonly Regex
            Choice = new Regex(
                @"\A\s*(?<" + AnswerStart0 + @">" + escAnswer + ")?" +
                @"\s*(?:\d+[\.:;]|\w+[\.:;]|" + Regex.Escape(UnorderedListMarkup) + ")" + // (un)ordered list
                @"\s*(?<" + AnswerStart1 + ">" + escAnswer + @")?" +
                @"\s*(?<" + ChoiceGroup + @">[^\*]+)" +
                @"\s*(?<" + AnswerEnd + ">" + escAnswer + @")?");
        public static Func<CollectibleWrapper, string> PictureFileName => x => x.FileName + ImageExtension;
        public static Func<CollectibleWrapper, string> TextFileName => x => x.Name;

        public string FileName { get; private set; }
        public bool IsRaritySet { get; private set; }

        public static readonly Dictionary<CsvColumn, DataField> CollectibleMapCsv =
            new Dictionary<CsvColumn, DataField>
        {
            { CsvColumn.FileName, new DataField("Naam", (c, v, r, e) => SetFileName(c, v, r, e))},
            { CsvColumn.DisplayName, new DataField("Weergavenaam", (c, v, r, e) => SetDisplayName(c, v, r, e)) },
            { CsvColumn.Points, new DataField("Punten", (c, v, r, e) => c.Points = GetInt(r, v, (int)CsvColumn.Points, "Punten", e)) },
            { CsvColumn.QuestionPoints, new DataField("Vraag", (c, v, r, e) => c.QuestionPoints = GetInt(r, v, (int)CsvColumn.Points, "Vraag", e)) },

            { CsvColumn.BioStad, new DataField(Biotope.Stad.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Stad)) },
            { CsvColumn.BioPark, new DataField(Biotope.Park.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Park)) },
            { CsvColumn.BioBos, new DataField(Biotope.Bos.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Bos)) },
            { CsvColumn.BioWeide, new DataField(Biotope.Weide.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Weide)) },
            { CsvColumn.BioWater, new DataField(Biotope.Water.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Water)) },
            { CsvColumn.BioZeeStrand,
                new DataField(Biotope.ZeeStrand.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.ZeeStrand)) },
            { CsvColumn.BioDuinen, new DataField(Biotope.Duinen.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Duinen)) },
            { CsvColumn.BioHeide, new DataField(Biotope.Heide.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Heide)) },
            { CsvColumn.BioTuin, new DataField(Biotope.Tuin.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Tuin)) },
            { CsvColumn.BioSpeeltuinSchoolplein,
                new DataField(Biotope.SpeeltuinSchoolplein.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.SpeeltuinSchoolplein)) },
            { CsvColumn.BioRivieren, new DataField(Biotope.Rivieren.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Rivieren)) },
            { CsvColumn.BioMoeras, new DataField(Biotope.Moeras.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Moeras)) },
            { CsvColumn.BioZeeuwseDelta, new DataField(Biotope.ZeeuwseDelta.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.ZeeuwseDelta)) },
            { CsvColumn.BioWadden, new DataField(Biotope.Wadden.Description(), (c, v, r, e) => SetBiotope(c, v, Biotope.Wadden)) },

            { CsvColumn.VeryCommon, new DataField(CollectibleRarity.VeryCommon.Description(), (c, v, r, e) =>
                    SetRarity(c, CollectibleRarity.VeryCommon, v, r, e )) },
            { CsvColumn.Common, new DataField(CollectibleRarity.Common.Description(), (c, v, r, e) =>
                    SetRarity(c, CollectibleRarity.Common, v, r, e )) },
            { CsvColumn.LessCommon, new DataField(CollectibleRarity.LessCommon.Description(), (c, v, r, e) =>
                    SetRarity(c, CollectibleRarity.LessCommon, v, r, e )) },
            { CsvColumn.Rare, new DataField(CollectibleRarity.Rare.Description(), (c, v, r, e) =>
                    SetRarity(c, CollectibleRarity.Rare, v, r, e )) },
            { CsvColumn.VeryRare, new DataField(CollectibleRarity.VeryRare.Description(), (c, v, r, e) =>
                    SetRarity(c, CollectibleRarity.VeryRare, v, r, e )) },

            { CsvColumn.January, new DataField(CalenderMonth.January.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.January)) },
            { CsvColumn.February, new DataField(CalenderMonth.February.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.February)) },
            { CsvColumn.March, new DataField(CalenderMonth.March.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.March)) },
            { CsvColumn.April, new DataField(CalenderMonth.April.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.April)) },
            { CsvColumn.May, new DataField(CalenderMonth.May.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.May)) },
            { CsvColumn.June, new DataField(CalenderMonth.June.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.June)) },
            { CsvColumn.July, new DataField(CalenderMonth.July.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.July)) },
            { CsvColumn.August, new DataField(CalenderMonth.August.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.August)) },
            { CsvColumn.September, new DataField(CalenderMonth.September.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.September)) },
            { CsvColumn.October, new DataField(CalenderMonth.October.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.October)) },
            { CsvColumn.November, new DataField(CalenderMonth.November.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.November)) },
            { CsvColumn.December, new DataField(CalenderMonth.December.Description(), (c, v, r, e) => SetMonth(c, v, CalenderMonth.December)) },
            { CsvColumn.AllYear, new DataField("Hele jaar aanwezig", (c, v, r, e) => SetMonth(c, v, EnumExtensions.GetAllYear())) }
        };

        private static bool _hasDisplayName;

        public CollectibleWrapper(bool hasDisplayName)
        {
            _hasDisplayName = hasDisplayName;
            IsRaritySet = false;
            CollectibleTypeTexts = new List<CollectibleTypeText>();
        }

        public void SetValue(int row, CsvColumn column, string value, List<OutputMessage> messages)
        {
            CollectibleMapCsv[column].Setter(this, value, row, messages);
        }

        public void SetText(int line, DescriptionPageHeader textId, string text, List<OutputMessage> messages)
        {
            if (CollectibleTypeTexts.Any(c => c.TextId == textId))
            {
                messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Text,
                    $"duplicaat sectie ({textId.Description()}) overgeslagen in [{TextFileName(this)}.txt/.md]", line);
            }
            else if (string.IsNullOrWhiteSpace(text))
            {
                messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Text,
                    $"Lege sectie ({textId.Description()}) in [{TextFileName(this)}.txt/.md]", line);
            }
            else
            {
                CollectibleTypeTexts.Add(new CollectibleTypeText
                {
                    TextId = textId,
                    Text = text
                });
            }
        }

        private static void SetFileName(CollectibleWrapper collectible, string value, int row, List<OutputMessage> messages)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Csv, 
                    "geen bestandsnaam", row + CsvHeaderLines);
                throw new InvalidDataException();
            }

            string name = value;
            Match match = Regex.Match(name, @"(\.[\w\d]{3,4})\z");

            if (match.Success)
            {
                name = name.Substring(0, name.Length - match.Value.Length);
            }

            if (name.Length + ImageExtension.Length > TagMaxLength)
            {
                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Csv,
                    $"bestandsnaam te lang ({name.Length + ImageExtension.Length}), max is {TagMaxLength}", row + CsvHeaderLines);
                throw new InvalidDataException();
            }

            collectible.FileName = name;

            if (!_hasDisplayName)
            {
                string display = Regex.Split(collectible.FileName, @"\d")[0].TrimEnd('-');
                SetDisplayName(collectible, display, row, messages);
            }

            collectible.TagName = PictureFileName(collectible);
        }

        private static void SetDisplayName(CollectibleWrapper collectible, string value, int row, List<OutputMessage> messages)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Csv,
                    $"geen weergavenaam", row + CsvHeaderLines);
                throw new InvalidDataException();
            }
            else if (value.Length > NameMaxLength)
            {
                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Csv,
                    $"weergavenaam te lang ({value.Length}), max is {NameMaxLength}", row + CsvHeaderLines);
                throw new InvalidDataException();
            }
            else
            {
                collectible.Name = value;
            }
        }

        private static int GetInt(int row, string value, int column, string columnName, List<OutputMessage> messages)
        {
            string error;

            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return int.Parse(value);
                }
                // else
                error = "geen waarde";
            }
            catch
            {
                error = "waarde niet numeriek";
            }

            messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Csv, error, row + CsvHeaderLines, column, columnName);

            return 0;
        }

        private static void SetRarity(
            CollectibleWrapper collectible, 
            CollectibleRarity rarity, 
            string value, 
            int row,
            List<OutputMessage> messages)
        {
            if (value.ToLower().Trim() == CsvCheckedValue)
            {
                if (collectible.IsRaritySet)
                {
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Csv,
                        $"zeldzaamheid \"{collectible.Rarity.Description()}\" overschreven door \"{rarity.Description()}\"", row + CsvHeaderLines);
                }

                collectible.Rarity = rarity;
                collectible.IsRaritySet = true;
            }
        }
    
        private static void SetBiotope(CollectibleWrapper collectible, string value, Biotope biotopeId)
        {
            if (value.ToLower().Trim() == CsvCheckedValue)
            {
                collectible.Biotopes |= biotopeId;
            }
        }
    
        private static void SetMonth(CollectibleWrapper collectible, string value, CalenderMonth monthId)
        {
            if (value.ToLower().Trim() == CsvCheckedValue)
            {
                collectible.MonthsAvailable |= monthId;
            }
        }
    }
}
