using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Accord.Imaging.Converters;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using AForge.Neuro.Learning;

namespace Polsl.Inf.Os2.WKiRO.ImageClassification
{
    public partial class Form1 : Form
    {
        #region Variables and ctor

        private const string Prefix = @"C:\Users\Szymon\Desktop\101_ObjectCategories";
        private static int _numCategories = 5; // can be up to 101
        private static int _unsupervisedEpochs = 200; // originally 200
        private static int _supervisedEpochs = 300; // originally 500
        private const int NumExamples = 30; // must be <= 31
        private const int NumTrain = 20; // must be < NUM_EXAMPLES to have something to test
        private const int WIDTH = 30; // standard width for images used here
        private const int HEIGHT = 20; // standard height for images used here
        private static readonly int[] Layers = { 600, 400, _numCategories, _numCategories }; // architecture of the net

        private static readonly ImageToArray Itoa = new ImageToArray(min: 0, max: 1);
        private static readonly ArrayToImage Atoi = new ArrayToImage(WIDTH, HEIGHT, min: 0.0, max: 1.0);
        private DeepBeliefNetwork _network;
        private Bitmap _imageToClassify;
        private string[] _categories;

        public Form1()
        {
            InitializeComponent();

            UpdateLayerDescription();
            txtUnsupervised.Text = _unsupervisedEpochs.ToString();
            txtSupervised.Text = _supervisedEpochs.ToString();
            txtCategories.Text = _numCategories.ToString();
        }

        #endregion

        private void chooseImage_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user clicks OK, load the picture that the user chose. 
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            var image = (Bitmap)Image.FromFile(openFileDialog1.FileName, true);
            _imageToClassify = ShrinkImage(image);
            pictureBox1.Load(openFileDialog1.FileName);
        }

        #region Done

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            var st = saveFileDialog1.OpenFile();

            _network.Save(st);
            st.Close();
        }

        private void LoadButton_Clicked(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                _network = DeepBeliefNetwork.Load(openFileDialog2.FileName);
            }
        }

        private static Bitmap ShrinkImage(Bitmap bmp)
        {
            Bitmap bmp2 = new Bitmap(WIDTH, HEIGHT);
            Graphics g = Graphics.FromImage(bmp2);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(bmp, 0, 0, bmp2.Width, bmp2.Height);
            g.Dispose();
            return bmp2;
        }

        private void GetData(out double[][] rInputs, out double[][] rOutputs, out double[][] testInputs, out double[][] testOutputs)
        {
            var inputs = new List<double[]>();
            var outputs = new List<double[]>();
            var tInputs = new List<double[]>();
            var tOutputs = new List<double[]>();
            var shortCats = new List<string>();

            var categories = Directory.GetDirectories(Prefix);
            var min = 10000;
            label1.Text = "";
            for (int i = 0; i < _numCategories; i++)
            {
                var c = categories[i];

                var split = c.Split('\\');
                shortCats.Add(split.Last());
                var files = Directory.GetFiles(c, "*.jpg");
                label1.Text += $"{c} => {files.Length} files.\n";
                label1.Refresh();
                if (files.Length < min) min = files.Length;

                int added = 0;
                foreach (string f in files)
                {
                    var image = (Bitmap)Image.FromFile(f, true);
                    if (image.Width < 300 || image.Height < 180) continue;

                    // crop the image
                    image = image.Clone(new Rectangle(0, 0, 300, Math.Min(180, 200)), image.PixelFormat);

                    // downsample the image to save memory
                    var smallImage = ShrinkImage(image);
                    image.Dispose();

                    double[] input;
                    Itoa.Convert(smallImage, out input);
                    smallImage.Dispose();

                    var output = new double[_numCategories];
                    output[i] = 1;

                    added++;

                    if (added <= NumTrain)
                    {
                        inputs.Add(input);
                        outputs.Add(output);
                    }
                    else
                    {
                        tInputs.Add(input);
                        tOutputs.Add(output);
                    }
                    if (added >= NumExamples) break;
                }
            }
            label1.Text += $"Number of categories: {categories.Length} min files: {min} number of short cats: {shortCats.Count}";

            listBox1.Items.Clear();
            for (int i = 0; i < shortCats.Count; i++)
                listBox1.Items.Add(shortCats[i] + ", " + i);

            _categories = shortCats.ToArray();
            rInputs = inputs.ToArray();
            rOutputs = outputs.ToArray();
            testInputs = tInputs.ToArray();
            testOutputs = tOutputs.ToArray();
        }

        private void train_Click(object sender, EventArgs e)
        {
            double[][] inputs;
            double[][] outputs;
            double[][] testInputs;
            double[][] testOutputs;
            GetData(out inputs, out outputs, out testInputs, out testOutputs);

            var sw = Stopwatch.StartNew();

            // Setup the deep belief network and initialize with random weights.
            _network = new DeepBeliefNetwork(inputs.First().Length, Layers);
            new GaussianWeights(_network).Randomize();
            _network.UpdateVisibleWeights();

            // Setup the learning algorithm.
            var teacher = new DeepBeliefNetworkLearning(_network)
            {
                Algorithm = (h, v, i) => new ContrastiveDivergenceLearning(h, v)
                {
                    LearningRate = 0.1,
                    Momentum = 0.5,
                    Decay = 0.001,
                }
            };

            // Setup batches of input for learning.
            int batchCount = Math.Max(1, inputs.Length / 100);
            // Create mini-batches to speed learning.
            int[] groups = Accord.Statistics.Tools.RandomGroups(inputs.Length, batchCount);
            double[][][] batches = inputs.Subgroups(groups);
            // Learning data for the specified layer.

            // Unsupervised learning on each hidden layer, except for the output layer.
            for (int layerIndex = 0; layerIndex < _network.Machines.Count - 1; layerIndex++)
            {
                teacher.LayerIndex = layerIndex;
                var layerData = teacher.GetLayerInput(batches);
                for (int i = 0; i < _unsupervisedEpochs; i++)
                {
                    double error = teacher.RunEpoch(layerData) / inputs.Length;
                    if (i % 10 == 0)
                    {
                        label1.Text = $"Layer: {layerIndex} Epoch: {i}, Error: {error}";
                        label1.Refresh();
                    }
                }
            }

            // Supervised learning on entire network, to provide output classification.
            var teacher2 = new BackPropagationLearning(_network)
            {
                LearningRate = 0.1,
                Momentum = 0.5
            };

            // Run supervised learning.
            for (int i = 0; i < _supervisedEpochs; i++)
            {
                double error = teacher2.RunEpoch(inputs, outputs) / inputs.Length;
                if (i % 10 == 0)
                {
                    label1.Text = $"Supervised: {i}, Error = {error}";
                    label1.Refresh();
                }
            }

            // Test the resulting accuracy.
            label1.Text = "";
            int correct = 0;
            for (int i = 0; i < testInputs.Length; i++)
            {
                double[] outputValues = _network.Compute(testInputs[i]);
                int y = GetResult(outputValues);
                int t = GetResult(testOutputs[i]);
                label1.Text += $"predicted: {y} actual: {t}\n";
                label1.Refresh();
                if (y == t)
                {
                    correct++;
                }
            }
            sw.Stop();

            label1.Text = $"Correct {correct}/{testInputs.Length}, {Math.Round(value: correct / (double)testInputs.Length * 100, digits: 2)}%";
            label1.Text += $"\nElapsed train+test time: {sw.Elapsed}";
            label1.Refresh();
        }

        private static int GetResult(double[] output)
        {
            return output.ToList().IndexOf(output.Max());
        }

        private void Classify(object sender, EventArgs e)
        {
            if (_imageToClassify == null)
            {
                label1.Text = "You didn't choose an image!\n";
                label1.Refresh();
                return;
            }

            double[] input;
            Itoa.Convert(_imageToClassify, out input);
            double[] output = _network.Compute(input);
            label1.Text = $"Prediction: {_categories[GetResult(output)]}";
            label1.Refresh();
        }

        #endregion

        #region Not important

        private void getDataButton_Clicked(object sender, EventArgs e)
        {
            double[][] inputs;
            double[][] outputs;
            double[][] testInputs;
            double[][] testOutputs;
            GetData(out inputs, out outputs, out testInputs, out testOutputs);
        }

        private void txtCategories_Changed(object sender, EventArgs e)
        {
            _numCategories = int.Parse(txtCategories.Text);
            // TODO: need to update LAYERS
            UpdateLayerDescription();
        }

        private void generateInputButton_Clicked(object sender, EventArgs e)
        {
            Recall(false);
        }

        private void Recall(bool reconstruct)
        {
            var sp = textBox1.Text.Split(',');
            if (sp.Length != 2)
            {
                label1.Text = @"You need to enter <neuron>,<layer>!";
                label1.Refresh();
                return;
            }
            try
            {
                int neuron = int.Parse(sp[0]);
                int layer = int.Parse(sp[1]);
                string c = (layer == Layers.Length - 1) ? listBox1.Items[neuron].ToString() : "(not a category)";

                var a = reconstruct ? new double[Layers[layer]] : new double[_numCategories];
                a[neuron] = 1;

                var r = reconstruct ? _network.Reconstruct(a, layer) : _network.GenerateInput(a);
                Bitmap bm;
                Atoi.Convert(r, out bm);

                label1.Text = $"Reconstructing {c}, length of reconstruction: {r.Length}";
                label1.Refresh();

                pictureBox1.Image = bm;
                pictureBox1.Refresh();
            }
            catch (Exception ex)
            {
                label1.Text = $"{ex.Message}\n{ex.StackTrace}\n";
                label1.Text += @"Reconstruction input params invalid. neuron should be < size of layer.";
                label1.Refresh();
            }
        }

        private void reconstructButton_Click(object sender, EventArgs e)
        {
            Recall(true);
        }

        private void txtUnsupervised_Changed(object sender, EventArgs e)
        {
            _unsupervisedEpochs = int.Parse(txtUnsupervised.Text);
            UpdateLayerDescription();
        }

        private void txtSupervised_Changed(object sender, EventArgs e)
        {
            _supervisedEpochs = int.Parse(txtSupervised.Text);
            UpdateLayerDescription();
        }

        private void UpdateLayerDescription()
        {
            label3.Text += "\n";
            for (int i = 0; i < Layers.Length; i++)
            {
                label3.Text += $"Layer {i} has {Layers[i]} neurons.\n";
            }
            label3.Refresh();
        }

        #endregion
    }
}
