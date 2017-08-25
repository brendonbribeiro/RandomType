using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RandType
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

		private static float GetFloat(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomFloat(config.Min.Float, config.Max.Float);
		}

		private static char GetChar(RandTypeSettings config)
		{
			return PrimitiveRandom.GenerateRandomChar();
		}

		private static byte GetByte(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomByte();
		}

		private static byte[] GetBytes(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomBytes(PrimitiveRandom.GetRandomInt(config.Min.ListSize, config.Max.ListSize));
		}

		private static long GetInt64(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomInt64(config.Min.Int64, config.Max.Int64);
		}

		private static TimeSpan GetTimeSpan(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomTimeSpan(config.Min.TimeSpan, config.Max.TimeSpan);
		}

		private static string GetString(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomString(config.Min.String, config.Max.String);
		}

		private static int GetInt32(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomInt(config.Min.Int32, config.Max.Int32);
		}

		private static bool GetBool(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomBool();
		}

		private static double GetDouble(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomDouble(config.Min.Double, config.Max.Double);
		}

		private static DateTime GetDate(RandTypeSettings config)
		{
			return PrimitiveRandom.GetRandomDateTime(config.Min.DateTime, config.Max.DateTime);
		}

		private static decimal GetDecimal(RandTypeSettings config)
		{
			return Convert.ToDecimal(PrimitiveRandom.GetRandomDouble(Convert.ToDouble(config.Min.Decimal), Convert.ToDouble(config.Max.Decimal)));
		}

		public static object Get(Type type, RandTypeSettings config)
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