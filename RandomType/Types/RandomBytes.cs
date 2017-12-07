using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomBytes
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type == typeof(byte[]) || type == typeof(byte?[]);
		}

		[Get]
		public static byte[] Get(Type type, RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomBytes(PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize));
		}
	}
}
