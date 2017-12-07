using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomEnum
	{
		[Match]
		public static bool Matches(Type type)
		{
			var utype = GetNullableType(type);
			return type.IsEnum || (utype != null && utype.IsEnum);
		}

		[Get]
		public static object Get(Type type, RandomTypeSettings config)
		{
			var t = GetNullableType(type);
			var enumList = Enum.GetValues(t);

			var randomEnum = enumList.GetValue(PrimitiveRandom.GetRandomInt32(0, enumList.Length));
			return randomEnum;
		}

		private static Type GetNullableType(Type type)
		{
			var nType = Nullable.GetUnderlyingType(type);
			return nType ?? type;
		}
	}
}
