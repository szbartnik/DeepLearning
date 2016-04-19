using System;
using System.Collections.Generic;
using System.IO;
using Wkiro.ImageClassification.Core.Models.Dto;
using Wkiro.ImageClassification.Mnist.Signatures;

namespace Wkiro.ImageClassification.Mnist
{
	public class DatasetImporter
	{
		public string DataPath { get; set; }
		public string LabelsPath { get; set; }

		public DatasetImporter(string dataPath, string labelsPath)
		{
			DataPath = dataPath;
			LabelsPath = labelsPath;
		}

		public InputsOutputsData ImportDataset ()
		{
			var data = new InputsOutputsData();
			var labels = readLabels();
			var digits = readDigits();

			for (int i = 0; i < labels.Length; i++)
			{
				var outputs = new double[10];
				outputs[labels[i]] = 1.0;
				data.AddData(digits[i], outputs);
			}

			return data;
		}

		private byte[] readLabels()
		{
			byte[] labels;
			using (var stream = new FileStream(LabelsPath, FileMode.Open))
			using (var reader = new BinaryReader(stream))
			{
				var signature = readLabelFileSignature(reader);
				labels = reader.ReadBytes(signature.Count);
			}
			return labels;
		}

		private List<double[]> readDigits()
		{
			var digits = new List<double[]>();
			using (var stream = new FileStream(DataPath, FileMode.Open))
			using (var reader = new BinaryReader(stream))
			{
				{
					var signature = readDataFileSignature(reader);
					var pixelsPerDigit = signature.RowWidth * signature.ColumnHeight;
					for (int i = 0; i < signature.Count; i++)
					{
						var normalizedDigit = new double[pixelsPerDigit];
						var digit = reader.ReadBytes(pixelsPerDigit);
						for (int p = 0; p < pixelsPerDigit; p++)
						{
							normalizedDigit[p] = normalizePixel(digit[p]);
						}
						digits.Add(normalizedDigit);
					}
				}
			}
			return digits;
		}

		private double normalizePixel (byte pixel)
		{
			double scale = 1.0 / 255.0;
			return pixel * scale;
		}

		private MnistLabelFileSignature readLabelFileSignature(BinaryReader reader)
		{
			var fileSignature = new MnistLabelFileSignature();
			fileSignature.MagicNumber = readInt32(reader);
			fileSignature.Count = readInt32(reader);
			return fileSignature;
		}

		private MnistDataFileSignature readDataFileSignature (BinaryReader reader)
		{
			var fileSignature = new MnistDataFileSignature();
			fileSignature.MagicNumber = readInt32(reader);
			fileSignature.Count = readInt32(reader);
			fileSignature.RowWidth = readInt32(reader);
			fileSignature.ColumnHeight = readInt32(reader);
			return fileSignature;
		}

		private int readInt32(BinaryReader reader)
		{
			var bytes = reader.ReadBytes(sizeof(int));
			Array.Reverse(bytes);
			return BitConverter.ToInt32(bytes, 0);
		}
	}
}
