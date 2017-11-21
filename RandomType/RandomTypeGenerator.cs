using RandomType.CustomRandom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace RandomType
{
	public class RandomTypeGenerator
	{
		/// <summary>
		/// Generate random values for entire model
		/// </summary>
		/// <typeparam name="T">The type you want to generate</typeparam>
		/// <returns></returns>
		public static T Generate<T>()
		{
			return GenerateRandomModel<T>(new RandomTypeSettings());
		}

		/// <summary>
		/// Generate a list of random models
		/// </summary>
		/// <typeparam name="T">The type you want to generate</typeparam>
		/// <returns></returns>
		public static List<T> GenerateList<T>()
		{
			var list = RandomList.Generate(typeof(T), new RandomTypeSettings());
			return (List<T>)list;
		}

		/// <summary>
		/// Generate random values for entire model
		/// </summary>
		/// <typeparam name="T">The type you want to generate</typeparam>
		/// <param name="configuration">Range configurator</param>
		/// <returns></returns>
		public static T Generate<T>(Action<RandomTypeSettings> configuration)
		{
			RandomTypeSettings defaultConfiguration = new RandomTypeSettings();
			configuration.Invoke(defaultConfiguration);
			return GenerateRandomModel<T>(defaultConfiguration);
		}

		/// <summary>
		/// Generate a list of random models
		/// </summary>
		/// <typeparam name="T">The type you want to generate</typeparam>
		/// <param name="configuration">Range configurator</param>
		/// <returns></returns>
		public static List<T> GenerateList<T>(Action<RandomTypeSettings> configuration)
		{
			RandomTypeSettings defaultConfiguration = new RandomTypeSettings();
			configuration.Invoke(defaultConfiguration);
			var list = RandomList.Generate(typeof(T), defaultConfiguration);
			return (List<T>)list;
		}

		internal static T GenerateRandomModel<T>(RandomTypeSettings configuration)
		{

			if (PrimitiveFuncs.Contains(typeof(T)))
			{
				return (T)PrimitiveFuncs.Get(typeof(T), configuration);
			}
			else if (RandomList.Validate(typeof(T)))
			{
				return (T)RandomList.Generate(typeof(T), configuration);
			}
			else if (RandomEnum.Validate(typeof(T)))
			{
				return (T)RandomEnum.Generate(typeof(T));
			}
			else if (RandomDictionary.Validate(typeof(T)))
			{
				return (T)RandomDictionary.Generate(typeof(T), configuration);

				//return (T)Activator.CreateInstance(typeof(T));
			}
			else
			{
				T model = (T)Activator.CreateInstance(typeof(T))/* new T()*/;
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
						if (RandomList.Validate(propType))
						{
							SetValue(model, prop, RandomList.Generate(propType, configuration));
						}
						else if (RandomEnum.Validate(propType))
						{
							SetValue(model, prop, RandomEnum.Generate(propType));
						}
						else
						{
							var method = typeof(RandomTypeGenerator).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(propType);
							var randomModel = method.Invoke(null, new object[] { configuration });
							SetValue(model, prop, randomModel);
						}
					}
				});

				return model;
			}
		}

		//private static bool IsEnum(Type type)
		//{
		//	var utype = Nullable.GetUnderlyingType(type);
		//	return type.IsEnum || (utype != null && utype.IsEnum);
		//}

		//private static Type GetNullableType(Type type)
		//{

		//	var nType = Nullable.GetUnderlyingType(type);
		//	return nType ?? type;
		//}

		//private static object GenerateEnum(Type type)
		//{
		//	var t = GetNullableType(type);
		//	var enumList = Enum.GetValues(t);

		//	var randomEnum = enumList.GetValue(PrimitiveRandom.GetRandomInt32(0, enumList.Length));
		//	return randomEnum;
		//}

		//private static bool IsList(Type type)
		//{
		//	Type tColl = typeof(ICollection<>);
		//	return type.IsGenericType && tColl.IsAssignableFrom(type.GetGenericTypeDefinition()) ||
		//		type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl);
		//}

		//private static IList GenerateList(Type type, RandomTypeSettings config)
		//{
		//	if (type.IsArray)
		//	{
		//		var arrayType = type.GetElementType();
		//		if (PrimitiveFuncs.Contains(arrayType))
		//		{
		//			return GeneratePrimitiveArray(arrayType, config);
		//		}
		//		else
		//		{
		//			return GenerateCustomArray(arrayType, config);
		//		}
		//	}
		//	else
		//	{
		//		var listType = type.GetGenericArguments()[0];
		//		if (PrimitiveFuncs.Contains(listType))
		//		{
		//			return GeneratePrimitiveList(listType, config);
		//		}
		//		else
		//		{
		//			return GenerateCustomList(listType, config);
		//		}
		//	}
		//}

		//private static IList GenerateCustomList(Type type, RandomTypeSettings config)
		//{
		//	IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

		//	var maxListSize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
		//	if (maxListSize > 0)
		//	{
		//		var method = typeof(RandomTypeGenerator).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(type);
		//		for (int i = 0; i < maxListSize; i++)
		//		{
		//			var randomModel = method.Invoke(null, new object[] { config });
		//			list.Add(randomModel);
		//		}
		//	}

		//	return list;
		//}

		//private static IList GeneratePrimitiveList(Type type, RandomTypeSettings config)
		//{
		//	IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

		//	var maxListSize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
		//	if (maxListSize > 0)
		//	{
		//		for (int i = 0; i < maxListSize; i++)
		//		{
		//			var randomValue = PrimitiveFuncs.Get(type, config);
		//			list.Add(randomValue);
		//		}
		//	}

		//	return list;
		//}

		//private static Array GenerateCustomArray(Type type, RandomTypeSettings config)
		//{
		//	var maxArraySize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
		//	Array array = Array.CreateInstance(type, maxArraySize);
		//	if (maxArraySize > 0)
		//	{
		//		var method = typeof(RandomTypeGenerator).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(type);
		//		for (int i = 0; i < maxArraySize; i++)
		//		{
		//			var randomModel = method.Invoke(null, new object[] { config });
		//			array.SetValue(randomModel, i);
		//		}
		//	}

		//	return array;
		//}

		//private static Array GeneratePrimitiveArray(Type type, RandomTypeSettings config)
		//{
		//	var maxArraySize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
		//	Array array = Array.CreateInstance(type, maxArraySize);
		//	if (maxArraySize > 0)
		//	{
		//		for (int i = 0; i < maxArraySize; i++)
		//		{
		//			var randomValue = PrimitiveFuncs.Get(type, config);
		//			array.SetValue(randomValue, i);
		//		}
		//	}

		//	return array;
		//}

		public static IDictionary GenerateDictionary(Type type, RandomTypeSettings config)
		{
			return new Dictionary<int, double>();
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
		private static Action<T, object> BuildUntypedSetter<T>(PropertyInfo propertyInfo)
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
