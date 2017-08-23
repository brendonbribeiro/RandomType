using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
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
					//prop.SetValue(model, PrimitiveFuncs.Get(propType, configuration));
					SetValue(model, prop, PrimitiveFuncs.Get(propType, configuration));
				}
				else
				{
					if (IsList(propType))
					{
						var listType = propType.GetGenericArguments()[0];
						if (PrimitiveFuncs.Contains(listType))
						{
							//prop.SetValue(model, GeneratePrimitiveList(listType, configuration));
							SetValue(model, prop, GeneratePrimitiveList(listType, configuration));

						}
						else
						{
							//prop.SetValue(model, GenerateCustomList(listType, configuration));
							SetValue(model, prop, GenerateCustomList(listType, configuration));
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



		private object GetValue<T>(T model, PropertyInfo prop)

		{

			Func<T, object> get = (Func<T, object>)

				Delegate.CreateDelegate(typeof(Func<T, object>), null,

					prop.GetGetMethod());

			return get(model);

		}
	}
}
