# DeepLearning

##### Reason of creation
The program has been created as a project in Computer Vision and Image Recognition course during MSc studies in Informatics on Silesian University of Technology.

##### Short description
The ImageClassification program gets classified images as input and using Deep Belief Network algorithm (DBN) trains network which can be later used to classify images, which the program haven't seen before, to the appropriate category. Internally the program uses Accord.NET framework.

##### Screenshot
![image](https://cloud.githubusercontent.com/assets/5238147/16058986/59c38e90-3281-11e6-819f-dcc9d5f538a5.png)

## End-user features
- Fully implemented Deep Belief Network learning using Accord.NET framework
- Two main stages of learning - unsupervised and supervised phases
- Fully configurable amount of neurons in each hidden layer of unsupervised phase and number of the hidden layers
- Automated division of input images on train and test data (necessary in supervised phase) and configurable ratio of the division
- Configurable processing dimensions of input data (all images need to be scaled to the same width and height values)
- Images preprocessing (before touching training phase). Following image preprocessors can be used:
 - Gray Scale
 - Auto Crop
 - Equalize Histogram
- Full control over selecting desired categories which will taka a part in training process
- Logging most important operations, user errors and unhandled exceptions to the Log box and log file
- Saving configuration of a training on program exit (parameters, paths etc.)
- Learned network saving and loading possibility
- Classification of loaded image to the appropriate category with showing the probabiliy of the association
- Cancelling and skipping different training phases in any moment of a training process
- User input validation

## Developer features
- C#6 and .NET 4.6.1
- GUI in WPF and using MVVM design pattern
- NLog as logging solution (configuration saved in App.xml file as custom config file section)
- Code divided physically and logically between Core and GUI part
- Nuget MahApps.Metro used to improve GUI style quality
- Exception handling
- Image preprocessors set is prepared to be extended in an easy way

## Example data to classify
- MNIST http://yann.lecun.com/exdb/mnist/
- 101 Categories - http://www.vision.caltech.edu/Image_Datasets/Caltech101/
- Cats & Dogs (available after registration) - https://www.kaggle.com/c/dogs-vs-cats/data

## Limitations
The learning quality REALLY! depends on quality of input data. If you have very inconsistent data (varying on image dimensions, contrast, brightness, aspect ratio) then you need to perform an extra work before you run ImageClassification program on it.

To preprocess your input training data you can perform one of following steps:
1. Use built-in image preprocessors listed above
2. Extend image preprocessors set to have ones which will meet your needs
3. Preprocess data outside ImageClassification program. For example here is source code of program we used in some cases (when we had to subtract images of similiar aspect ratio from bigger set) - http://pastebin.com/5hftuKrD

Pay attention checking if each category of training set has similiar amount of samples. The divergence here may cause the quality of the training will be degraded
