using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Search
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var ci = new System.Globalization.CultureInfo(Properties.Settings.Default.CurrentLanguage);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
