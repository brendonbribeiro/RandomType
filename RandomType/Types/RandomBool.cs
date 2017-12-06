using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomBool
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(bool) || type == typeof(bool?);
		}

		[Get]
		public static bool Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomBool();
		}
	}
}
