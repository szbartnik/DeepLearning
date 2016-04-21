using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Wkiro.ImageClassification.Gui.ViewModels;

namespace Wkiro.ImageClassification.Gui.Views
{
    public partial class MainWindowView : MetroWindow
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindowView(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = viewModel;

            var iconUri = new Uri("pack://application:,,,/ImageClassification.Gui;component/Resources/TitleBarIcon.ico", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }

        private void OnLogTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            LogTextBox.CaretIndex = LogTextBox.Text.Length;
            LogTextBox.ScrollToEnd();
        }

        private void MainWindowView_OnClosing(object sender, CancelEventArgs e)
        {
            _viewModel.SaveConfiguration();
        }
    }
}
