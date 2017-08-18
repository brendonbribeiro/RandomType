using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandType
{
	public class RandType
	{
		public static T Generate<T>() where T : class, new()
		{
			return GenerateRandomModel<T>(new RandTypeSettings());
		}

		public static T Generate<T>(Action<RandTypeSettings> configuration) where T : class, new()
		{
			RandTypeSettings defaultConfiguration = new RandTypeSettings();
			configuration.Invoke(defaultConfiguration);
			return GenerateRandomModel<T>(defaultConfiguration);
		}

		private static T GenerateRandomModel<T>(RandTypeSettings configuration) where T : class, new()
		{
			T model = new T();
			var type = typeof(T);
			var props = GetPublicProperties(type);
			props.ForEach(prop =>
			{

				var propType = prop.PropertyType;
				if (propType == typeof(string))
				{
					prop.SetValue(model, PrimitiveRandom.GetRandomString(configuration.Min.String, configuration.Max.String));
				}
				else if (propType == typeof(bool) || propType == typeof(bool?))
				{
					prop.SetValue(model, PrimitiveRandom.GetRandomBool());
				}
				else if (propType == typeof(int) || propType == typeof(int?))
				{
					prop.SetValue(model, PrimitiveRandom.GetRandomInt(configuration.Min.Int32, configuration.Max.Int32));
				}
				else if (propType == typeof(double) || propType == typeof(double?))
				{
					prop.SetValue(model, PrimitiveRandom.GetRandomDouble(configuration.Min.Double, configuration.Max.Double));
				}
				else if (propType == typeof(double) || propType == typeof(double?))
				{
					prop.SetValue(model, PrimitiveRandom.GetRandomDouble(configuration.Min.Double, configuration.Max.Double));
				}
				else if (propType == typeof(DateTime) || propType == typeof(DateTime?))
				{
					prop.SetValue(model, PrimitiveRandom.GetRandomDateTime(configuration.Min.DateTime, configuration.Max.DateTime));
				}
				else if (propType == typeof(decimal) || propType == typeof(decimal?))
				{
					prop.SetValue(model, Convert.ToDecimal(PrimitiveRandom.GetRandomDouble(Convert.ToDouble(configuration.Min.Decimal), Convert.ToDouble(configuration.Max.Decimal))));
				}

				Type tColl = typeof(ICollection<>);
				if (propType.IsGenericType && tColl.IsAssignableFrom(propType.GetGenericTypeDefinition()) ||
					propType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl))
				{
					var listType = propType.GetGenericArguments()[0];

					var constructedListType = typeof(List<>).MakeGenericType(listType);
					IList instance = (IList)Activator.CreateInstance(constructedListType);

					//var listRandomModel = Activator.CreateInstance(listType);

					var k = typeof(RandType).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(listType);

					var maxListSize = PrimitiveRandom.GetRandomInt(configuration.Min.Int32, configuration.Max.Int32);
					if (maxListSize > 0)
					{
						for (int i = 0; i < maxListSize; i++)
						{
							var result = k.Invoke(null, new object[] { configuration });

							instance.Add(result);
						}
					}

					prop.SetValue(model, instance);
				}
			});

			return model;
		}

		private static List<PropertyInfo> GetPublicProperties(Type type)
		{
			var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(prop => prop.GetSetMethod() != null)
				.ToList();
			return props;
		}
	}

	public class RandTypeSettings
	{
		public RandTypeRangeSettings Min { get; set; }

		public RandTypeRangeSettings Max { get; set; }

		public RandTypeSettings()
		{
			Min = new RandTypeRangeSettings()
			{
				Double = 0,
				Int32 = 0,
				String = 2,
				DateTime = DateTime.MinValue,
				Decimal = 0,
				ListSize = 30
			};

			Max = new RandTypeRangeSettings()
			{
				Double = 5000,
				Int32 = 5000,
				String = 30,
				DateTime = DateTime.Now,
				Decimal = 5000,
				ListSize = 0
			};
		}
	}

	public class RandTypeRangeSettings
	{
		public int String { get; set; }

		public int Int32 { get; set; }

		public double Double { get; set; }

		public decimal Decimal { get; set; }

		public DateTime DateTime { get; set; }

		public int ListSize { get; set; }
	}
}
