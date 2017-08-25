using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandType.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var rand = RandType.Generate<Part>();
		}

		public class Part
		{
			public long Long { get; set; }

			public TimeSpan TimeSpan { get; set; }

			public byte[] ByteArray { get; set; }

			public byte Byte { get; set; }

			public List<byte> ByteList { get; set; }

			public List<Batata> BatataList { get; set; }

			public Batata[] BatataArray { get; set; }

			public int[] IntArray { get; set; }

			public char Char { get; set; }

			public float Float { get; set; }
		}

		public class Batata
		{
			public int Hue { get; set; }
		}
	}
}
