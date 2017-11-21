using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.CustomRandom
{
	internal class RandomEnum
	{
		public static bool Validate(Type type)
		{
			var utype = GetNullableType(type);
			return type.IsEnum || (utype != null && utype.IsEnum);
		}

		public static object Generate(Type type)
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
