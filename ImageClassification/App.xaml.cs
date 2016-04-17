using System.Windows;
using Polsl.Inf.Os2.WKiRO.ImageClassification.ViewModels;
using Polsl.Inf.Os2.WKiRO.ImageClassification.Views;

namespace Polsl.Inf.Os2.WKiRO.ImageClassification
{
    public partial class App : Application
    {
        public App()
        {
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
