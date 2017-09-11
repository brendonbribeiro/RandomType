using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RandType
{
	public class RandType
	{
		/// <summary>
		/// Generate random values for entire model (including lists)
		/// </summary>
		/// <typeparam name="T">The type you want to generate</typeparam>
		/// <returns></returns>
		public static T Generate<T>() where T : class, new()
		{
			return GenerateRandomModel<T>(new RandTypeSettings());
		}

		/// <summary>
		/// Generate random values for entire model (including lists)
		/// </summary>
		/// <typeparam name="T">The type you want to generate</typeparam>
		/// <param name="configuration">Range configurator</param>
		/// <returns></returns>
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
					SetValue(model, prop, PrimitiveFuncs.Get(propType, configuration));
				}
				else
				{
					if (IsList(propType))
					{
						GenerateList(propType, configuration, model, prop);
					}
					else
					{
						var method = typeof(RandType).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(propType);
						var randomModel = method.Invoke(null, new object[] { configuration });
						SetValue(model, prop, randomModel);
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

		private static void GenerateList<TModel>(Type type, RandTypeSettings config, TModel model, PropertyInfo prop)
		{
			if (type.IsArray)
			{
				var arrayType = type.GetElementType();
				if (PrimitiveFuncs.Contains(arrayType))
				{
					SetValue(model, prop, GeneratePrimitiveArray(arrayType, config));
				}
				else
				{
					SetValue(model, prop, GenerateCustomArray(arrayType, config));
				}
			}
			else
			{
				var listType = type.GetGenericArguments()[0];
				if (PrimitiveFuncs.Contains(listType))
				{
					SetValue(model, prop, GeneratePrimitiveList(listType, config));
				}
				else
				{
					SetValue(model, prop, GenerateCustomList(listType, config));
				}
			}
		}

		private static IList GenerateCustomList(Type type, RandTypeSettings config)
		{
			IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

			var maxListSize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
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

			var maxListSize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
			if (maxListSize > 0)
			{
				for (int i = 0; i < maxListSize; i++)
				{
					var randomValue = PrimitiveFuncs.Get(type, config);
					list.Add(randomValue);
				}
			}

			return list;
		}

		public static Array GenerateCustomArray(Type type, RandTypeSettings config)
		{
			var maxArraySize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
			Array array = Array.CreateInstance(type, maxArraySize);
			if (maxArraySize > 0)
			{
				var method = typeof(RandType).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(type);
				for (int i = 0; i < maxArraySize; i++)
				{
					var randomModel = method.Invoke(null, new object[] { config });
					array.SetValue(randomModel, i);
				}
			}

			return array;
		}

		public static Array GeneratePrimitiveArray(Type type, RandTypeSettings config)
		{
			var maxArraySize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
			Array array = Array.CreateInstance(type, maxArraySize);
			if (maxArraySize > 0)
			{
				for (int i = 0; i < maxArraySize; i++)
				{
					var randomValue = PrimitiveFuncs.Get(type, config);
					array.SetValue(randomValue, i);
				}
			}

			return array;
		}

		private static List<PropertyInfo> GetPublicProperties(Type type)
		{
			var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(prop => prop.GetSetMethod() != null)
				.ToList();
			return props;
		}

		private static void SetValue<T>(T model, PropertyInfo prop, object value)
		{
			var setter = BuildUntypedSetter<T>(prop);
			setter(model, value);
		}

		//https://stackoverflow.com/questions/17660097/is-it-possible-to-speed-this-method-up
		public static Action<T, object> BuildUntypedSetter<T>(PropertyInfo propertyInfo)
		{
			var targetType = propertyInfo.DeclaringType;
			var methodInfo = propertyInfo.GetSetMethod();
			var exTarget = Expression.Parameter(targetType, "t");
			var exValue = Expression.Parameter(typeof(object), "p");
			var exBody = Expression.Call(exTarget, methodInfo,
			Expression.Convert(exValue, propertyInfo.PropertyType));
			var lambda = Expression.Lambda<Action<T, object>>(exBody, exTarget, exValue);
			var action = lambda.Compile();
			return action;
		}
	}
}