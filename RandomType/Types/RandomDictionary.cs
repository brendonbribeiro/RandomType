using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomDictionary
	{
		[Match]
		public static bool Matches(Type type)
		{
			return type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type);
		}

		[Get]
		public static IDictionary Get(Type type, RandomTypeSettings config)
		{
			var dic = (IDictionary)Activator.CreateInstance(type);
			var keyType = type.GetGenericArguments()[0];
			var valueType = type.GetGenericArguments()[1];

			var maxListSize = PrimitiveRandom.GetRandomInt32(config.Min.DictionarySize, config.Max.DictionarySize);

			if (maxListSize > 0)
			{
				while (dic.Count < maxListSize)
				{
					var randomKey = RandomTypeGenerator.Generate(keyType, config);
					if (!dic.Contains(randomKey))
					{
						var randomValue = RandomTypeGenerator.Generate(valueType, config);
						dic.Add(randomKey, randomValue);
					}
				}
			}

			return dic;
		}
	}
}
