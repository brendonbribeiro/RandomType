using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomDouble
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(double) || type == typeof(double?);
		}

		[Get]
		public static double Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomDouble();
		}
	}
}
