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
				if (PrimitiveFuncs.Contains(propType))
				{
					prop.SetValue(model, PrimitiveFuncs.Get(propType, configuration));
				}
				else
				{
					if (IsList(propType))
					{
						var listType = propType.GetGenericArguments()[0];
						if (PrimitiveFuncs.Contains(listType))
						{
							prop.SetValue(model, GeneratePrimitiveList(listType, configuration));
						}
						else
						{
							prop.SetValue(model, GenerateCustomList(listType, configuration));
						}
					}
				}
			});

			return model;
		}

		private static bool IsList(Type type)
		{
			Type tColl = typeof(ICollection<>);
			return type.IsGenericType && tColl.IsAssignableFrom(type.GetGenericTypeDefinition()) ||
				type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl);
		}

		private static IList GenerateCustomList(Type type, RandTypeSettings config)
		{
			IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

			var maxListSize = PrimitiveRandom.GetRandomInt(config.Min.ListSize, config.Max.ListSize);
			if (maxListSize > 0)
			{
				var method = typeof(RandType).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(type);
				for (int i = 0; i < maxListSize; i++)
				{
					var randomModel = method.Invoke(null, new object[] { config });
					list.Add(randomModel);
				}
			}

			return list;
		}

		private static IList GeneratePrimitiveList(Type type, RandTypeSettings config)
		{
			IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

			var maxListSize = PrimitiveRandom.GetRandomInt(config.Min.ListSize, config.Max.ListSize);
			if (maxListSize > 0)
			{
				//var method = typeof(RandType).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(type);
				for (int i = 0; i < maxListSize; i++)
				{
					var randomValue = PrimitiveFuncs.Get(type, config);
					list.Add(randomValue);
				}
			}

			return list;
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
				ListSize = 0
			};

			Max = new RandTypeRangeSettings()
			{
				Double = 5000,
				Int32 = 5000,
				String = 30,
				DateTime = DateTime.Now,
				Decimal = 5000,
				ListSize = 30
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
