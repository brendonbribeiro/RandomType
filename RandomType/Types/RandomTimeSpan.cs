using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomTimeSpan
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(TimeSpan) || type == typeof(TimeSpan?);
		}

		[Get]
		public static TimeSpan Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomTimeSpan();
		}
	}
}
