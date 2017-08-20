using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandType
{
	public class PrimitiveFuncs
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
				return tp;
			}
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
