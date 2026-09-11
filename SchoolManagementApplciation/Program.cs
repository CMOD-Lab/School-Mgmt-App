using System;
using System.Windows.Forms;

namespace SchoolManagementApplciation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // .NET 8: ApplicationConfiguration.Initialize() replaces the manual
            // Application.EnableVisualStyles() + Application.SetCompatibleTextRenderingDefault(false) calls
            ApplicationConfiguration.Initialize();
            Application.Run(new WelcomeScreen());
        }
    }
}
