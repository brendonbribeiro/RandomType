using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomChar
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(char) || type == typeof(char?);
		}

		[Get]
		public static char Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GenerateRandomChar();
		}
	}
}
