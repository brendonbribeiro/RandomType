using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomDecimal
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(decimal) || type == typeof(decimal?);
		}

		[Get]
		public static decimal Get(Type type, RandomTypeSettings config)
		{
			return Convert.ToDecimal(PrimitiveRandom.GetRandomDouble());
		}
	}
}
