using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using Wkiro.ImageClassification.Gui.ViewModels;

namespace Wkiro.ImageClassification.Gui.Views
{
    public partial class MainWindowView : MetroWindow
    {
        public MainWindowView(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            var iconUri = new Uri("pack://application:,,,/ImageClassification.Gui;component/Resources/TitleBarIcon.ico", UriKind.RelativeOrAbsolute);
            Icon = BitmapFrame.Create(iconUri);
        }

        private void OnLogTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            LogTextBox.CaretIndex = LogTextBox.Text.Length;
            LogTextBox.ScrollToEnd();
        }

        public void Run()
        {
            //var availableCategories = dataProvider.GetAvailableCategories();
            //var selectedCategories = availableCategories.Take(5).ToList();

            //var learningSet = dataProvider.GetLearningSetForCategories(selectedCategories);

            //var trainer = new Trainer(new TrainerConfiguration
            //{
            //    Layers = new[] { 600, 400, 5, 5 },
            //    InputsOutputsData = learningSet.TrainingData.ToInputOutputsDataNative(),
            //}, new Logger());

            //trainer.RunTraining1(new Training1Parameters
            //{
            //    Momentum = 0.5,
            //    Decay = 0.001,
            //    LearningRate = 0.1,
            //    UnsupervisedEpochs = 200,
            //});

            //trainer.RunTraining2(new Training2Parameters
            //{
            //    Momentum = 0.5,
            //    LearningRate = 0.1,
            //    SupervisedEpochs = 300,
            //});

            //trainer.CheckAccuracy(learningSet.TestData.ToInputOutputsDataNative());

            //var classifier = new Classifier(trainer.NeuralNetwork, new ClassifierConfiguration
            //{
            //    Categories = selectedCategories,
            //}, new Logger());

            //var imagePrepared = dataProvider.PrepareImageByPath("path");
            //classifier.ClassifyToCategory(imagePrepared);
        }
    }
}
