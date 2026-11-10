using System;
using System.ComponentModel;

namespace CollectibleUploadToolCon
{
    [Flags]
    public enum Options
    {
        None = 0,
        [Description("sla bestaande [E]igenschappen/tekst over en negeer missende teksten")]
        SkipData = 1,
        [Description("sla bestaande [A]fbeeldingen over en negeer missende afbeeldingen")]
        SkipImages = 2,
        [Description("[T]est, alleen lezen, geen opslag")]
        RunTest = 4,
        // 'Weergave'
        [Description("[W]aarschuwingen")]
        ShowWarnings = 8,
        [Description("[F]outen")]
        ShowErrors = 16,
        [Description("[O]vergeslagen items")]
        ShowSkippedFiles = 32,
        [Description("[V]ervangen items")]
        ShowReplacedItems = 64,
        [Description("[S]uccesvolle items")]
        ShowSuccesfulItems = 128
    }

    public class InputArgs
    {
        public string CsvFile { get; set; }
        public string TxtDir { get; set; }
        public string ImgDir { get; set; }
        public Options Options { get; set; }
    }
}
