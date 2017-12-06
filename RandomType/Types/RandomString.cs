using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomString
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(string);
		}

		[Get]
		public static string Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomString(config.Min.String, config.Max.String);
		}
	}
}
