using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RandType.Console
{
	class Program
	{
		static void Main(string[] args)
		{

			var rand = RandType.Generate<Part>();
			//var list = new List<int>();
			//for (int i = 0; i < 500000; i++)
			//{
			//	list.Add(NextInt(Int32.MinValue, Int32.MaxValue));
			//}
			//var k = NextInt(Int32.MinValue, Int32.MaxValue);
		}

		//http://www.vcskicks.com/code-snippet/rng-int.php
		private static int NextInt(int min, int max)
		{
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buffer = new byte[4];

			rng.GetBytes(buffer);
			int result = BitConverter.ToInt32(buffer, 0);

			return new Random(result).Next(min, max);
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
