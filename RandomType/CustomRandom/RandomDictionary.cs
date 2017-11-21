using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RandomType.CustomRandom
{
	internal class RandomDictionary
	{
		public static bool Validate(Type type)
		{
			return type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type);
		}

		public static IDictionary Generate(Type type, RandomTypeSettings config)
		{
			IDictionary dic = (IDictionary)Activator.CreateInstance(type);
			var keyType = type.GetGenericArguments()[0];
			var valueType = type.GetGenericArguments()[1];

			var maxListSize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);

			if (maxListSize > 0)
			{
				var method = typeof(RandomTypeGenerator).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static);

				var keyMethod = method.MakeGenericMethod(keyType);
				var valueMethod = method.MakeGenericMethod(valueType);

				while (dic.Count < maxListSize)
				{
					var randomKey = keyMethod.Invoke(null, new object[] { config });
					if (!dic.Contains(randomKey))
					{
						var randomValue = valueMethod.Invoke(null, new object[] { config });
						dic.Add(randomKey, randomValue);
					}
					else
					{
						Console.WriteLine(randomKey + " Já foi");
					}
				}
			}


			//if (maxListSize > 0)
			//{
			//	var method = typeof(RandomTypeGenerator).GetMethod("GenerateRandomModel", BindingFlags.NonPublic | BindingFlags.Static);

			//	var keyMethod = method.MakeGenericMethod(keyType);
			//	var randomKey = keyMethod.Invoke(null, new object[] { config });

			//	var valueMethod = method.MakeGenericMethod(valueType);
			//	var randomValue = valueMethod.Invoke(null, new object[] { config });

			//	dic.Add(randomKey, randomValue);
			//}
			//RandomTypeGenerator.GenerateRandomModel<>(config);
			return dic;
		}
	}
}
