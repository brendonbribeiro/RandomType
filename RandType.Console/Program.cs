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

			//public byte ByteProp { get; set; }
		}
	}
}
