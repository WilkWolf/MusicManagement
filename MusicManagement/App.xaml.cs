using System.Threading;
using System.Windows;

namespace MusicManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    SetLanguageDictionary();
        //}

        //private void SetLanguageDictionary()
        //{
        //    switch (Thread.CurrentThread.CurrentCulture.ToString())
        //    {
        //        case "pl-PL":
        //            Language.Resources.Culture = new System.Globalization.CultureInfo("pl-PL");
        //            break;
        //        default://default english because there can be so many different system language, we rather fallback on english in this case.
        //            Language.Resources.Culture = new System.Globalization.CultureInfo("pl-EN");
        //            break;
        //    }

        //}
    }

}
