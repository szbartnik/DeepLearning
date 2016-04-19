using System;

namespace Wkiro.ImageClassification.Mnist.Signatures
{
	internal class MnistDataFileSignature
	{
		private int _magicNumber;

		public int MagicNumber
		{
			get { return _magicNumber; }
			set
			{
				if (value != 2051)
					throw new ArgumentException("Invalid Magic Number for label file.");
				_magicNumber = value;
			}
		}
		public int Count { get; set; }
		public int RowWidth { get; set; }
		public int ColumnHeight { get; set; }
	}
}
