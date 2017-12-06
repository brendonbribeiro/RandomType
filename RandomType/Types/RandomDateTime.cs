using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomDateTime
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(DateTime) || type == typeof(DateTime?);
		}

		[Get]
		public static DateTime Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomDateTime();
		}
	}
}
