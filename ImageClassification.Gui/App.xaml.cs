using System.Windows;
using Wkiro.ImageClassification.Gui.ViewModels;
using Wkiro.ImageClassification.Gui.Views;

namespace Wkiro.ImageClassification.Gui
{
    public partial class App : Application
    {
        public App()
        {
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            Startup += OnAppStartup;
        }    

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            var viewModel = new MainWindowViewModel(true);
            var view = new MainWindowView(viewModel);
            view.Show();
        }
    }
}
