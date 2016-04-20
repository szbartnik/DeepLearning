using System;
using System.Collections.Generic;

namespace Wkiro.ImageClassification.Core.Models.Dto
{
    public class InputsOutputsData
    {
        public List<double[]> Inputs { get; }
        public List<double[]> Outputs { get; }
        public int Count => Inputs.Count;

        public InputsOutputsData()
        {
            Inputs = new List<double[]>();
            Outputs = new List<double[]>();
        }

        public void AddData(double[] inputs, double[] outputs)
        {
            Inputs.Add(inputs);
            Outputs.Add(outputs);
        }

        public void AddData(InputsOutputsData inputsOutputsData)
        {
            Inputs.AddRange(inputsOutputsData.Inputs);
            Outputs.AddRange(inputsOutputsData.Outputs);
        }

        public InputOutputsDataNative ToInputOutputsDataNative()
        {
            return new InputOutputsDataNative(Inputs.ToArray(), Outputs.ToArray());
        }
    }

    public class InputOutputsDataNative
    {
        public double[][] Inputs { get; }
        public double[][] Outputs { get; }

        public InputOutputsDataNative(double[][] inputs, double[][] outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }
    }
}
