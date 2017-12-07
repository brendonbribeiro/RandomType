using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomFloat
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(float) || type == typeof(float?);
		}

		[Get]
		public static float Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomFloat();
		}
	}
}
