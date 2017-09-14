using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RandomType
{
	internal class PrimitiveFuncs
	{
		private static MethodInfo GetMethod(string name)
		{
			return typeof(PrimitiveFuncs).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
		}

		private static List<MethodTypes> TypeMethods
		{
			get
			{
				var tp = new List<MethodTypes>();
				tp.Add(new MethodTypes(GetMethod("GetString"), typeof(string)));
				tp.Add(new MethodTypes(GetMethod("GetInt32"), typeof(int), typeof(int?)));
				tp.Add(new MethodTypes(GetMethod("GetBool"), typeof(bool), typeof(bool?)));
				tp.Add(new MethodTypes(GetMethod("GetDouble"), typeof(double), typeof(double?)));
				tp.Add(new MethodTypes(GetMethod("GetDate"), typeof(DateTime), typeof(DateTime?)));
				tp.Add(new MethodTypes(GetMethod("GetDecimal"), typeof(decimal), typeof(decimal?)));
				tp.Add(new MethodTypes(GetMethod("GetInt64"), typeof(Int64), typeof(Int64?)));
				tp.Add(new MethodTypes(GetMethod("GetTimeSpan"), typeof(TimeSpan), typeof(TimeSpan?)));
				tp.Add(new MethodTypes(GetMethod("GetByte"), typeof(byte), typeof(byte?)));
				tp.Add(new MethodTypes(GetMethod("GetBytes"), typeof(byte[]), typeof(byte?[]), typeof(ICollection<byte>)));
				tp.Add(new MethodTypes(GetMethod("GetBytesList"), typeof(ICollection<byte>)));
				tp.Add(new MethodTypes(GetMethod("GetChar"), typeof(char), typeof(char?)));
				tp.Add(new MethodTypes(GetMethod("GetFloat"), typeof(float), typeof(float?)));

				return tp;
			}
		}

		private static float GetFloat(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomFloat();
		}

		private static char GetChar(RandomTypeSettings config)
		{
			return PrimitiveRandom.GenerateRandomChar();
		}

		private static byte GetByte(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomByte();
		}

		private static byte[] GetBytes(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomBytes(PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize));
		}

		private static long GetInt64(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomInt64();
		}

		private static TimeSpan GetTimeSpan(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomTimeSpan();
		}

		private static string GetString(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomString(config.Min.String, config.Max.String);
		}

		private static int GetInt32(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomInt32(config.Min.Int32, config.Max.Int32);
		}

		private static bool GetBool(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomBool();
		}

		private static double GetDouble(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomDouble();
		}

		private static DateTime GetDate(RandomTypeSettings config)
		{
			return PrimitiveRandom.GetRandomDateTime();
		}

		private static decimal GetDecimal(RandomTypeSettings config)
		{
			return Convert.ToDecimal(PrimitiveRandom.GetRandomDouble());
		}

		public static object Get(Type type, RandomTypeSettings config)
		{
			var method = TypeMethods.First(m => m.Types.Contains(type)).Method;
			var obj = method.Invoke(null, new object[] { config });
			return obj;
		}

		public static bool Contains(Type type)
		{
			return TypeMethods.Any(m => m.Types.Contains(type));
		}
	}
}
