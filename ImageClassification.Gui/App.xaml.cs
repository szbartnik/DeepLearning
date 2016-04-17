using System.Windows;
using Wkiro.ImageClassification.ViewModels;
using Wkiro.ImageClassification.Views;

namespace Wkiro.ImageClassification
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
