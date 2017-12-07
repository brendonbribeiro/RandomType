using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomInt32
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(int) || type == typeof(int?);
		}

		[Get]
		public static int Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomInt32(config.Min.Int32, config.Max.Int32);
		}
	}
}
