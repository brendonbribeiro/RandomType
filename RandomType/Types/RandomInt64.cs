using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomInt64
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(Int64) || type == typeof(Int64?);
		}

		[Get]
		public static Int64 Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomInt64();
		}
	}
}
