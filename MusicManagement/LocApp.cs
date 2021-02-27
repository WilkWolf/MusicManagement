using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace MusicManagement
{
    public class LocApp : Application
    {
        [STAThread]
        public static void Main()
        {
            App app = new App();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Closed += Wnd_Closed;
            app.Run(mainWindow);
        }

        private static void Wnd_Closed(object sender, EventArgs e)
        {
            MainWindow mainWindow = sender as MainWindow;
            if (!string.IsNullOrEmpty(mainWindow.LangSwitch))
            {
                string lang = mainWindow.LangSwitch;

                mainWindow.Closed -= Wnd_Closed;

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

                mainWindow = new MainWindow();
                mainWindow.Closed += Wnd_Closed;
                mainWindow.Show();
                mainWindow.InformationTextBox.Text = MusicManagement.Resources.Resources.ChangedLanguage;
            }
            else
            {
                Current.Shutdown();
            }
        }
    }
}