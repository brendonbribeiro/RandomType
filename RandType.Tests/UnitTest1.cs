using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RandType.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			var rand = RandType.Generate<Part>();

		}

		public class Part
		{
			public long Id { get; set; }

			public TimeSpan Interval { get; set; }
		}
	}
}
