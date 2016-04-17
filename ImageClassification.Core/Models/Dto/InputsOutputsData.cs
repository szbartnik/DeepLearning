using System;
using System.Collections.Generic;

namespace Wkiro.ImageClassification.Core.Models.Dto
{
    internal class InputsOutputsData
    {
        public void AddData(InputsOutputsData trainingData)
        {
            throw new System.NotImplementedException();
        }

        public InputOutputsDataNative ToInputOutputsDataNative()
        {
            throw new System.NotImplementedException();
        }

        public double Count { get; set; }
        public List<double[]> Inputs { get; set; }
        public List<double[]> Outputs { get; set; }

        internal void AddData(double[] v1, double[] v2)
        {
            throw new NotImplementedException();
        }
    }

    public class InputOutputsDataNative
    {
        public double[][] Inputs { get; set; }
        public double[][] Outputs { get; set; }
    }
}
