using Speurzoekers.Common.Domain.Collectible;
using Speurzoekers.Common.Domain.Enums;
using Speurzoekers.Common.Helper;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CollectibleUploadToolCon
{
    class Program
    {
        private const Options OptionsShow = Options.ShowWarnings;

        private static SpeurzoekersDbConnection _speurzoekersDbCon;
        private static bool _skipImgSizeCheck;

        static void Main(string[] args)
        {
            Console.Title = "De Speurzoekers";
            InputArgs input = ReadInput(args);

            if (input == null) // no further actions required
            {
                Console.ReadKey();
                return;
            }
            else
            {
                _skipImgSizeCheck = !CollectibleWrapper.ImageExtension.Equals(".png", StringComparison.OrdinalIgnoreCase);

                if (!input.Options.HasFlag(Options.RunTest))
                {
                    string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    _speurzoekersDbCon = new SpeurzoekersDbConnection(basePath,
                        new CollectibleWrapperMapper(),
                        new CollectibleEntityMapper());

                    try
                    {
                        _speurzoekersDbCon.CheckSqlConnection();
                        _speurzoekersDbCon.CheckValiditySqlScheme();
                    }
                    catch (SpeurzoekersDbConnection.SpeurzoekersDbConException ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
#if DEBUG
                        Console.WriteLine(string.Format(Messages.DebugStartError, ex.Message));
#else
                    Console.WriteLine(string.Format(Messages.CriticalStartError, ex.Message));
#endif
                        Console.ResetColor();
                        Console.ReadKey();
#if !DEBUG
                    return;
#endif
                    }
                }
            }

            List<CollectibleWrapper> collectibleList = new List<CollectibleWrapper>();
            List<OutputMessage> messages = new List<OutputMessage>();
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            CancellationToken cancel = cancelSource.Token;

            Task spinner = Task.Run(() => Spinner(cancel));
            Exception exception = null;

            try
            {
                ReadData(input, collectibleList, messages).Wait();

                if (!input.Options.HasFlag(Options.RunTest))
                {
                    Save(input, collectibleList, messages).Wait();
                }
                else
                {
                    RunTest(input, collectibleList, messages);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            cancelSource.Cancel();
            spinner.Wait();

            if (exception == null)
            {
                ShowOutput(messages, input.Options);

                Console.WriteLine();

                if (input.Options.HasFlag(Options.RunTest))
                {
                    Console.WriteLine("Uitlezen voltooid");
                }
                else
                {
                    Console.WriteLine("Uitlezen en opslaan voltooid");
                }
            }
            else
            {
                if (exception.InnerException.GetType() == typeof(UploadToolException))
                {
                    Console.WriteLine(exception.InnerException.Message);
                }
                else
                {
                    Console.WriteLine($"Onbekende fout: {exception.InnerException.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("Uitlezen/opslaan gefaald");
            }

            Console.ReadKey();
        }

        private static InputArgs ReadInput(string[] args)
        {
            InputArgs input;

            if (args.Length == 3 || args.Length == 4)
            {
                input = new InputArgs
                {
                    CsvFile = args[0],
                    TxtDir = args[1],
                    ImgDir = args[2]
                };

                if (args.Length == 4)
                {
                    input.Options = ReadOptionsChoice(args[3].TrimStart('-'));
                }
                else
                {
                    input.Options = Options.None;
                }

                return input;
            }
            else if (args.Length == 2)
            {
                Export(args[0], args[1]);
                return null;
            }
            else if (!args.Any())
            {
                return new InputArgs
                {
                    CsvFile = GetPath(".csv-bestand", true),
                    TxtDir = GetPath("tekstbestanden map", false),
                    ImgDir = GetPath("afbeeldingen map", false),
                    Options = GetOptionsChoice()
                };
            }
            else
            {
                ShowHelp();
                return null;
            }
        }

        private static async Task ReadData(InputArgs input, List<CollectibleWrapper> collectibleList, List<OutputMessage> messages)
        {
            await ReadCsv(input.CsvFile, collectibleList, messages);

            RemoveDoubles(collectibleList, messages);
            await ReadTxt(input.TxtDir, collectibleList, messages, input.Options.HasFlag(Options.SkipData));

            if (!input.Options.HasFlag(Options.SkipImages))
            {
                CheckImgFiles(input.ImgDir, collectibleList, messages);
            }
        }

        private static async Task Spinner(CancellationToken cancel)
        {
            string spinner = "-\\|/";
            Console.Write("Bezig ");

            for (int i = 0; !cancel.IsCancellationRequested; i = ++i % 4)
            {
                Console.Write(spinner[i]);

                await Task.Delay(150);
                Console.Write("\b");
            }

            Console.WriteLine("... Klaar");
            Console.WriteLine();
        }

        private static void Export(string option, string path)
        {
            StringBuilder contentsBuilder = new StringBuilder();
            option = option.Substring(1).ToLower();

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Console.WriteLine("Map niet gevonden.");
                return;
            }

            switch (option)
            {
                case "csv":
                    if (!path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    {
                        path += ".csv";
                    }

                    string[] cols = GetColumns().ToArray();

                    contentsBuilder.Append("Eerste rij genegeerd");
                    contentsBuilder.AppendLine(new string(';', cols.Length));
                    contentsBuilder.AppendJoin(';', cols);
                    break;
                case "txt":
                case "md":
                    if (option == "txt" && !path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        path += ".txt";
                    }

                    if (option == "md" && !path.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                    {
                        path += ".md";
                    }

                    contentsBuilder.Append(CollectibleWrapper.HeaderPrefix);
                    contentsBuilder.AppendJoin("\n\n" + CollectibleWrapper.HeaderPrefix, GetHeaders());
                    contentsBuilder.AppendLine();
                    contentsBuilder.AppendLine();
                    contentsBuilder.AppendLine(CollectibleWrapper.HeaderPrefix + CollectibleWrapper.QuestionHeader);
                    contentsBuilder.AppendLine($"{CollectibleWrapper.UnorderedListMarkup}Foute keuze");
                    contentsBuilder.AppendLine($"{CollectibleWrapper.UnorderedListMarkup}" +
                        $"{CollectibleWrapper.AnswerMarkup}Goede keuze{CollectibleWrapper.AnswerMarkup}");
                    contentsBuilder.Append($"{CollectibleWrapper.UnorderedListMarkup}Foute keuze");
                    break;
                default:
                    Console.WriteLine("Optie niet herkend.");
                    return;
            }

            File.WriteAllText(path, contentsBuilder.ToString());

            Console.WriteLine("Bestand geëxporteerd.");
        }

        private static void ShowHelp()
        {
            string name = AppDomain.CurrentDomain.FriendlyName;

            Console.WriteLine("----- DE SPEURZOEKERS -----");
            Console.WriteLine();
            Console.WriteLine(name + "                             Vraagt om paden/opties");
            Console.WriteLine();
            Console.WriteLine(name + " -help                       Dit help menu");
            Console.WriteLine();
            Console.WriteLine(name + " -csv \"[pad/naam bestand]\"   Genereer een .csv-bestand met alle ondersteunde kolomnamen");
            Console.WriteLine(name + " -txt \"[pad/naam bestand]\"   Genereer een .txt-bestand met alle ondersteunde titels");
            Console.WriteLine(name + " -md  \"[pad/naam bestand]\"   Genereer een .md-bestand met alle ondersteunde titels");
            Console.WriteLine();
            Console.WriteLine(name + " \"[.csv-bestand]\" \"[tekstbestanden map]\" \"[afbeeldingen map]\" (-[opties])");
            Console.WriteLine($"\t\"[.csv-bestand]\"         Pad naar .csv-bestand, deze lijst wordt aangehouden voor het uploaden");
            Console.WriteLine($"\t\"[tekstbestanden map]\"   Pad naar de map met overige teksten (.txt of .md, 1 bestand per item)");
            Console.WriteLine($"\t\"[afbeeldingen map]\"     Pad naar de map met de afbeeldingen ({CollectibleWrapper.ImageExtension})");
            Console.WriteLine($"\t(-[opties])              Optionele opties (meerdere/geen mogelijk, gebruik gemarkeerde letter per optie):");

            ShowOptions("\t                       ");

            Console.WriteLine();
            Console.WriteLine("----- VELDEN .CSV-BESTAND -----");
            Console.WriteLine($"Verwacht aantal kopregels: {CollectibleWrapper.CsvHeaderLines}, " +
                $"met kolomnamen op rij {CollectibleWrapper.CsvColumnNameLine + 1}.");
            Console.WriteLine($"Verwachte velden in het .csv-bestand ('{CollectibleWrapper.CsvSeparator}' gescheiden), gelezen adhv. kolomnaam:");
            Console.WriteLine();
            Console.WriteLine(string.Join("; ", GetColumns()));
            Console.WriteLine();
            Console.WriteLine("----- VELDEN TEKSTBESTAND -----");
            Console.WriteLine("Verwachte velden in een tekstbestand, gelezen adhv. titel(naam).");
            Console.Write($"Titels worden gemaarkeerd met KOP1 (\"{CollectibleWrapper.HeaderPrefix}\")");
            Console.WriteLine(" Lege/ontbrekende titels zijn mogelijk:");
            Console.WriteLine();
            Console.WriteLine(string.Join("; ", GetHeaders()));
            Console.WriteLine();
            Console.Write($"En de titel \"{CollectibleWrapper.QuestionHeader}\".");
            Console.WriteLine($" Met keuzes gemarkeerd als een (on)georderde lijst en het antwoord(en) dikgedrukt.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Druk een toets om verder te gaan...");
        }

        private static IEnumerable<string> GetColumns()
        {
            return CollectibleWrapper.CollectibleMapCsv.Values.Select(x => x.ColumnName);
        }

        private static IEnumerable<string> GetHeaders()
        {
            return Enum.GetValues(typeof(DescriptionPageHeader))
                .Cast<DescriptionPageHeader>()
                .Select(x => x.Description());
        }

        private static string GetPath(string requestedPath, bool isCsv)
        {
            while (true)
            {
                string fileText = isCsv ? " (inclusief bestand)" : string.Empty;
                string path;

                Console.WriteLine($"Vul pad{fileText} naar {requestedPath} in:");

                while (true)
                {
                    path = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        break;
                    }
                }

                Console.Write("...");

                if (isCsv && !path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    path += ".csv";
                }

                if ((isCsv && File.Exists(path)) || (!isCsv && Directory.Exists(path)))
                {
                    Console.WriteLine(" gevonden");
                    Console.WriteLine();
                    return path;
                }
                else
                {
                    Console.WriteLine(" niet gevonden!");
                    Console.WriteLine();
                }
            }
        }

        private static Options GetOptionsChoice()
        {
            Console.WriteLine("Opties (meerdere/geen mogelijk, gebruik gemarkeerde letter per optie):");

            ShowOptions(string.Empty);

            Console.Write("Keuze: ");

            string choice = Console.ReadLine();
            Options result = ReadOptionsChoice(choice);

            Console.WriteLine();

            return result;
        }

        private static void ShowOptions(string prefix)
        {
            Console.WriteLine();

            for (int i = 1; i <= Math.Pow(2, Enum.GetNames(typeof(Options)).Length - 2); i *= 2)
            {
                if (i == (int)OptionsShow)
                {
                    Console.WriteLine();
                    Console.WriteLine(prefix + "Weergave opties:");
                }

                Console.WriteLine($"{prefix}\t- {((Options)i).Description()}");
            }
        }

        private static Options ReadOptionsChoice(string options)
        {
            Options choice = Options.None;

            options = options.ToLower();

            if (options.Contains('t'))
            { choice |= Options.RunTest; }

            if (options.Contains('e'))
            { choice |= Options.SkipData; }

            if (options.Contains('a'))
            { choice |= Options.SkipImages; }

            if (options.Contains('s'))
            { choice |= Options.ShowSuccesfulItems; }

            if (options.Contains('o'))
            { choice |= Options.ShowSkippedFiles; }

            if (options.Contains('v'))
            { choice |= Options.ShowReplacedItems; }

            if (options.Contains('w'))
            { choice |= Options.ShowWarnings; }

            if (options.Contains('f'))
            { choice |= Options.ShowErrors; }

            return choice;
        }

        private static void RemoveDoubles(List<CollectibleWrapper> collectibleList, List<OutputMessage> messages)
        {
            IEnumerable<string> doubles = collectibleList
                .GroupBy(s => s.FileName)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key);

            foreach (string key in doubles)
            {
                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Csv, $"niet unieke bestandsnaam; [{key}], item overgeslagen");
                collectibleList.RemoveAll(x => x.FileName == key);
            }

            doubles = collectibleList
                .GroupBy(s => s.Name)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key);

            foreach (string key in doubles)
            {
                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Csv, $"niet unieke naam; [{key}], item overgeslagen");
                collectibleList.RemoveAll(x => x.Name == key);
            }
        }

        private static async Task ReadCsv(string path, List<CollectibleWrapper> collectibleList, List<OutputMessage> messages)
        {
            string[] csvContent;

            try
            {
                csvContent = await File.ReadAllLinesAsync(path);
            }
            catch
            {
                throw new UploadToolException(".csv-bestand niet leesbaar (zorg dat het bestand gesloten is), geen verdere acties mogelijk.");
            }

            if (!csvContent[CollectibleWrapper.CsvColumnNameLine].Contains(CollectibleWrapper.CsvSeparator))
            {
                throw new UploadToolException($"Geen kolom-scheidingsteken ('{CollectibleWrapper.CsvSeparator}') gevonden in het .csv-bestand.");
            }

            string nameColumn = CollectibleWrapper.CollectibleMapCsv[CsvColumn.FileName].ColumnName;
            string[] headerColumns = csvContent[CollectibleWrapper.CsvColumnNameLine]
                .Split(CollectibleWrapper.CsvSeparator)
                .Select(x => x.Trim())
                .ToArray();
            bool hasDisplayName = 
                headerColumns.Contains(CollectibleWrapper.CollectibleMapCsv[CsvColumn.DisplayName].ColumnName);

            if (!headerColumns.Contains(nameColumn, StringComparer.OrdinalIgnoreCase))
            {
                throw new UploadToolException($"Geen kolom [{nameColumn}] gevonden in het .csv-bestand.");
            }

            int readColumns = headerColumns.Length;
            List<CsvColumn?> headerColumnList = new List<CsvColumn?>(readColumns);
            var expectedHeaders = Enum.GetValues(typeof(CsvColumn)).Cast<CsvColumn>();

            for (int i = 0; i < readColumns; i++)
            {
                string col = headerColumns[i];

                if (string.IsNullOrWhiteSpace(col))
                {
                    headerColumnList.Add(null);
                }
                else
                {
                    CsvColumn index = CollectibleWrapper.CollectibleMapCsv.Keys
                        .FirstOrDefault(x => string.Equals(CollectibleWrapper.CollectibleMapCsv[x].ColumnName, col,
                            StringComparison.OrdinalIgnoreCase));

                    if (index != 0)
                    { headerColumnList.Add(index); }
                    else
                    {
                        headerColumnList.Add(null);
                        messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Csv, 
                            $"kolom {OutputMessageExtensions.GetLetter(i)} ({col}) niet herkent");
                    }
                }
            }

            foreach (CsvColumn col in expectedHeaders)
            {
                if (!headerColumnList.Contains(col))
                {
                    string name = CollectibleWrapper.CollectibleMapCsv[col].ColumnName;
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Csv,
                            $"geen kolom [{name}] gevonden in het .csv-bestand.");
                }

            }

            csvContent = csvContent.Skip(CollectibleWrapper.CsvHeaderLines).ToArray();

            for (int row = 0; row < csvContent.Length; row++)
            {
                string[] values = csvContent[row].Split(';');

                if (values.Length != readColumns)
                {
                    messages.AddMessage(ErrorLevel.Error, ErroneousFile.Csv,
                        "incorrect aantal velden (verwacht " + readColumns + ", ingelezen " + values.Length + ")",
                        row + CollectibleWrapper.CsvHeaderLines);
                    messages.AddSkippedItem(values[(int)CsvColumn.FileName]);
                    continue; // discard this row
                }

                CollectibleWrapper collectible = new CollectibleWrapper(hasDisplayName);

                try
                {
                    for (int column = 0; column < values.Length; column++)
                    {
                        if (headerColumnList[column] != null)
                        {
                            collectible.SetValue(row, headerColumnList[column].Value, values[column], messages);
                        }
                    }
                }
                catch (InvalidDataException)
                {
                    messages.AddSkippedItem(values[(int)CsvColumn.FileName]);
                    continue; // discard this row
                }

                if (!collectible.IsRaritySet)
                {
                    collectible.Rarity = CollectibleWrapper.DefaultRarity;
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Csv,
                        "zeldzaamheid is niet gedefinieerd", row + CollectibleWrapper.CsvHeaderLines);
                }

                if (collectible.MonthsAvailable == 0)
                {
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Csv,
                        "beschikbaarheid (maanden) is niet gedefinieerd", row + CollectibleWrapper.CsvHeaderLines);
                }

                if (collectible.Biotopes == 0)
                {
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Csv,
                        "aanwezigheid (biotopen) is niet gedefinieerd", row + CollectibleWrapper.CsvHeaderLines);
                }

                collectibleList.Add(collectible);
            }
        }

        private static async Task ReadTxt(string path, List<CollectibleWrapper> collectibleList, List<OutputMessage> messages, bool skipData)
        {
            for (int x = 0; x < collectibleList.Count; x++)
            {
                CollectibleWrapper collectible = collectibleList[x];
                string file = CollectibleWrapper.TextFileName(collectible) + ".txt", filePath = Path.Combine(path, file);

                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(path, CollectibleWrapper.TextFileName(collectible) + ".md");

                    if (!File.Exists(filePath))
                    {
                        if (!skipData)
                        {
                            messages.AddMessage(ErrorLevel.Error, ErroneousFile.Text, 
                                $"[{CollectibleWrapper.TextFileName(collectible) + ".txt/.md"}] ontbreekt");
                            messages.AddSkippedItem(CollectibleWrapper.TextFileName(collectible));

                            collectibleList.Remove(collectible);
                            x--;
                        }

                        continue;
                    }
                }

                string allText = await File.ReadAllTextAsync(filePath);

                allText = Regex.Replace(allText, @"(?<format>[\*~]+)(?<text>[^\\\n\*~]+)(?<newLine>[\n\\]\n)(?:[\*~]+)",
                    "${format}${text}${format}${newLine}"); // * Italic ** Bold *** Bold/Italic ~~ Strikethrough

                string[] txtContent = allText.Split('\n');
                string textBlock = string.Empty;
                DescriptionPageHeader? header = null;
                int headerLineNr = -1, nrOfQuestions = 0, nrOfAnswers = 0;
                bool isQuestion = false;
                Dictionary<DescriptionPageHeader, string> headers = Enum.GetValues(typeof(DescriptionPageHeader))
                    .Cast<DescriptionPageHeader>()
                    .ToDictionary(k => k, v => v.Description());

                for (int y = 0; y < txtContent.Length; y++)
                {
                    KeyValuePair<DescriptionPageHeader, string> field;
                    string line = txtContent[y].TrimStart();

                    if (line.StartsWith(CollectibleWrapper.HeaderPrefix))
                    {
                        line = line.Substring(CollectibleWrapper.HeaderPrefix.Length).Trim();

                        if (header.HasValue)
                        {
                            collectible.SetText(headerLineNr, header.Value, textBlock.Trim(), messages);
                            textBlock = string.Empty;
                        }
                        else if (isQuestion)
                        {
                            collectible.QuestionText = textBlock.Trim();
                            textBlock = string.Empty;
                        }

                        if (string.Equals(line, CollectibleWrapper.QuestionHeader, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (nrOfQuestions > 0)
                            {
                                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Text, $"meerdere vragen gevonden in [" + file + "]");
                                messages.AddSkippedItem(collectible.FileName);

                                collectibleList.Remove(collectible);
                                y = int.MaxValue; // stop inner loop
                                x--;

                                continue;
                            }

                            isQuestion = true;
                            header = null;
                            nrOfQuestions++;

                            collectible.CollectibleTypeChoices = new List<CollectibleTypeChoice>();
                            collectible.QuestionText = string.Empty;
                        }
                        else
                        {
                            isQuestion = false;

                            field = headers.FirstOrDefault(cw => string.Equals(line, cw.Value, StringComparison.InvariantCultureIgnoreCase));

                            if (field.Value != null)
                            {
                                header = field.Key;
                                headerLineNr = y;
                            }
                            else
                            {
                                header = null;

                                messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Text, $"titel [{line}] in [{file}] niet herkend en overgeslagen", y);
                            }
                        }
                    }
                    else if (isQuestion)
                    {
                        Match choice = CollectibleWrapper.Choice.Match(txtContent[y]);

                        if (choice.Success)
                        {
                            bool isAnswer = choice.Groups[CollectibleWrapper.AnswerEnd].Length > 0 &&
                                (choice.Groups[CollectibleWrapper.AnswerStart0].Length > 0 || choice.Groups[CollectibleWrapper.AnswerStart1].Length > 0);
                            string text = choice.Groups[CollectibleWrapper.ChoiceGroup].Value.Trim();

                            if (isAnswer)
                            {
                                nrOfAnswers++;
                            }

                            collectible.CollectibleTypeChoices.Add(new CollectibleTypeChoice
                            {
                                Text = text,
                                IsAnswer = isAnswer
                            });
                        }
                        else
                        {
                            textBlock += txtContent[y] + "\n";
                        }
                    }
                    else if (header.HasValue)
                    {
                        textBlock += txtContent[y] + "\n";
                    }
                }

                if (header.HasValue)
                {
                    collectible.SetText(headerLineNr, header.Value, textBlock, messages);
                }
                else if (isQuestion)
                {
                    collectible.QuestionText = textBlock.Trim();
                }

                if (headerLineNr < 0)
                {
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Text, $"geen geldige titels herkend in [{file}]");
                }
                else if (nrOfQuestions == 0)
                {
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Text, $"geen vraag gevonden in [{file}]");
                }
                else if (nrOfAnswers == 0 || (!CollectibleWrapper.AllowMultipleAnswers && nrOfAnswers > 1))
                {
                    messages.AddMessage(ErrorLevel.Warning, ErroneousFile.Text, $"geen uniek antwoord ({nrOfAnswers}) gevonden in [{file}]");
                }
            }
        }

        private static void CheckImgFiles(string path, List<CollectibleWrapper> collectibleList, List<OutputMessage> messages)
        {
            for (int i = 0; i < collectibleList.Count; i++)
            {
                CollectibleWrapper collectible = collectibleList[i];
                string filePath = Path.Combine(path, collectible.TagName);

                if (!File.Exists(filePath))
                {
                    messages.AddMessage(ErrorLevel.Error, ErroneousFile.Image,
                        $"[{collectible.TagName}] ontbreekt");
                    messages.AddSkippedItem(collectible.TagName);

                    collectibleList.Remove(collectible);
                    i--;

                    continue;
                }
            }
        }

        private static async Task Save(InputArgs input, List<CollectibleWrapper> collectibleList, List<OutputMessage> messages)
        {
            if (!collectibleList.Any())
            { return; }

            bool skipData = input.Options.HasFlag(Options.SkipData), skipImg = input.Options.HasFlag(Options.SkipImages);

            for (int i = 0; i < collectibleList.Count; i++)
            {
                CollectibleWrapper col = collectibleList[i];
                string filePath = Path.Combine(input.ImgDir, col.TagName);

                // File existence is already checked if skipImg == false
                if (!skipImg || File.Exists(filePath))
                {
                    using FileStream stream = File.OpenRead(filePath);

                    if (_skipImgSizeCheck || !CheckImageSize(stream, col.TagName, messages))
                    {
                        collectibleList.Remove(col);
                        i--;
                    }

                    await _speurzoekersDbCon.BlobRepository.SaveCollectiblePicture(col.TagName, stream, !skipImg);
                }
            }

            List<string> names = _speurzoekersDbCon.CollectibleRepository.GetExistingNames(collectibleList.Select(x => x.Name)).Result;
            names.ForEach(x => messages.AddReplacedItem(x));

            if (skipData)
            {
                collectibleList.RemoveAll(x => names.Contains(x.Name, StringComparer.OrdinalIgnoreCase));
            }

            await _speurzoekersDbCon.CollectibleRepository.SaveCollectibles(collectibleList, !skipData);

            collectibleList.ForEach(x => messages.AddSuccessfulItem(x.Name));
        }

        private static void RunTest(InputArgs input, List<CollectibleWrapper> collectibleList, List<OutputMessage> messages)
        {
            if (_skipImgSizeCheck)
            {
                return;
            }

            for (int i = 0; i < collectibleList.Count; i++)
            {
                CollectibleWrapper col = collectibleList[i];
                string filePath = Path.Combine(input.ImgDir, col.TagName);
                using FileStream stream = File.OpenRead(filePath);

                if (!CheckImageSize(stream, col.TagName, messages))
                {
                    collectibleList.Remove(col);
                    i--;
                }
            }

            collectibleList.ForEach(x => messages.AddSuccessfulItem(x.Name));
        }

        private static void ShowOutput(IEnumerable<OutputMessage> messages, Options summary)
        {
            IEnumerable<OutputMessage> list = messages.Where(e => ((int)e.Level & (int)summary) > 0);

            foreach (OutputMessage message in list)
            {
                switch (message.Level)
                {
                    case ErrorLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case ErrorLevel.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case ErrorLevel.SkippedFile:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                }

                Console.WriteLine(message.Text);
                Console.ResetColor();
            }
        }

        public static bool CheckImageSize(FileStream stream, string tagName, List<OutputMessage> messages)
        {
            byte[] bytes = new byte[8];
            int width = 0, height = 0;

            stream.Seek(16, SeekOrigin.Begin); // jump to the 16th byte where width and height information is stored
            stream.Read(bytes, 0, 8); // width (4 bytes), height (4 bytes)

            for (int i = 0; i <= 3; i++)
            {
                width = bytes[i] | width << 8;
                height = bytes[i + 4] | height << 8;
            }

            if (width != CollectibleWrapper.ImageWidth || height != CollectibleWrapper.ImageHeight)
            {
                messages.AddMessage(ErrorLevel.Error, ErroneousFile.Image,
                    $"[{tagName}] is niet {CollectibleWrapper.ImageWidth}X{CollectibleWrapper.ImageHeight}");
                messages.AddSkippedItem(tagName);

                return false;
            }
            // else
            return true;
        }
    }
}
