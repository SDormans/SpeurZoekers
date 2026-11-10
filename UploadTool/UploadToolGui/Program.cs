using UploadToolGui.Dto.Enums;
using System;
using System.Linq;
using System.Windows.Forms;

namespace UploadToolGui
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppModeOptions appMode = AppModeOptions.Collectibles;

            if (args.Any())
            {
                if (args[0].ToLower().Contains("activit")) // + "eit", "y" or "ies"
                {
                    appMode = AppModeOptions.Activity;
                }
                else if (args[0].ToLower().Contains("avatar"))
                {
                    appMode = AppModeOptions.Avatars;
                }
            }

            Application.Run(new MainForm(appMode));
        }
    }
}
