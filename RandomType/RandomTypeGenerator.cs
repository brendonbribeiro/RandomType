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
			return Generate<T>(new RandomTypeSettings());
		}

		/// <summary>
		/// Generate random values for entire model
		/// </summary>
		/// <typeparam name="T">The type you want to generate</typeparam>
		/// <param name="configuration">Range configurator</param>
		/// <returns></returns>
		public static T Generate<T>(Action<RandomTypeSettings> configuration)
		{
			RandomTypeSettings cfg = new RandomTypeSettings();
			configuration.Invoke(cfg);
			return Generate<T>(cfg);
		}

		public static T Generate<T>(RandomTypeSettings configuration)
		{
			return (T)Generate(typeof(T), configuration);
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

		public static List<T> GenerateList<T>(RandomTypeSettings configuration)
		{
			var list = RandomList.Generate(typeof(T), configuration);
			return (List<T>)list;
		}

		public static object GenerateList(Type type, RandomTypeSettings configuration)
		{
			var list = RandomList.Generate(type, configuration);
			return list;
		}

		//private static (bool valueSet, object value) GetTypeRandomValue(Type type, RandomTypeSettings configuration)
		//{
		//	switch (type)
		//	{
		//		case Type t when PrimitiveFuncs.Contains(type):
		//			return (true, PrimitiveFuncs.Get(type, configuration));
		//		case Type i when RandomList.Validate(type):
		//			return (true, RandomList.Generate(type, configuration));
		//		case Type i when RandomEnum.Validate(type):
		//			return (true, RandomEnum.Generate(type));
		//		case Type i when RandomDictionary.Validate(type):
		//			return (true, RandomDictionary.Generate(type, configuration));
		//		default:
		//			return (false, null);
		//	}
		//}

		private static KeyValuePair<bool, object> GetTypeRandomValue(Type type, RandomTypeSettings configuration)
		{
			switch (type)
			{
				case Type t when PrimitiveFuncs.Contains(type):
					return new KeyValuePair<bool, object>(true, PrimitiveFuncs.Get(type, configuration));
				case Type i when RandomList.Validate(type):
					return new KeyValuePair<bool, object>(true, RandomList.Generate(type, configuration));
				case Type i when RandomEnum.Validate(type):
					return new KeyValuePair<bool, object>(true, RandomEnum.Generate(type));
				case Type i when RandomDictionary.Validate(type):
					return new KeyValuePair<bool, object>(true, RandomDictionary.Generate(type, configuration));
				default:
					return new KeyValuePair<bool, object>(false, null);
			}
		}

		public static object Generate(Type type, RandomTypeSettings configuration)
		{
			var tv = GetTypeRandomValue(type, configuration);
			if (tv.Key)
			{
				return tv.Value;
			}
			else
			{
				object model = Activator.CreateInstance(type);
				var props = GetPublicProperties(type);
				props.ForEach(prop =>
				{

					var propType = prop.PropertyType;
					tv = GetTypeRandomValue(propType, configuration);

					if (tv.Key)
					{
						SetValue(model, prop, tv.Value);
					}
					else
					{
						var randomModel = Generate(propType, configuration);
						SetValue(model, prop, randomModel);
					}
				});

				return model;
			}
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
			//var setter = BuildUntypedSetter<T>(prop);
			//setter(model, value);

			//TODO FASTER
			prop.SetValue(model, value);
		}

		//private static void SetValue(object model, PropertyInfo prop, object value)
		//{
		//	if (value != null)
		//	{
		//		var method = typeof(RandomTypeGenerator).GetMethod("SetTypedValue", BindingFlags.Static | BindingFlags.NonPublic);
		//		var meth = method.MakeGenericMethod(model.GetType(), value.GetType());
		//		meth.Invoke(null, new object[] { model, prop, value });

		//	}

		//}

		//private static void SetTypedValue<T, TOut>(T model, PropertyInfo prop, TOut value)
		//{
		//	Action<T, TOut> set = (Action<T, TOut>)
		//		Delegate.CreateDelegate(typeof(Action<T, TOut>), null,
		//			prop.GetSetMethod());

		//	set(model, value);
		//}

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
