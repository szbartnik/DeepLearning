using System;
using System.Collections.Generic;
using System.Linq;

namespace Wkiro.ImageClassification.Core.Models.Dto
{
    public class InputsOutputsData
    {
        public List<double[]> Inputs { get; private set; }
        public List<double[]> Outputs { get; private set; }
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

        public void Shuffle()
        {
            var random = new Random();
            var newIndices = Enumerable.Range(0, Inputs.Count).OrderBy(x => random.Next()).ToArray();
            var newInputs = new List<double[]>();
            var newOutputs = new List<double[]>();
            foreach (var newIndex in newIndices)
            {
                newInputs.Add(Inputs[newIndex]);
                newOutputs.Add(Outputs[newIndex]);
            }
            Inputs = newInputs;
            Outputs = newOutputs;
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
