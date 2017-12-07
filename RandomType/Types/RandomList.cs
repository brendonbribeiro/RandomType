using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RandomType.Types
{
	[RandomType]
	internal class RandomList
	{
		[Match]
		public static bool Matches(Type type)
		{
			Type tColl = typeof(ICollection<>);
			return (type.IsGenericType && tColl.IsAssignableFrom(type.GetGenericTypeDefinition()) ||
				type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl))
				&& !typeof(IDictionary).IsAssignableFrom(type);
		}

		[Get]
		public static IList Get(Type type, RandomTypeSettings config)
		{
			if (type.IsArray)
			{
				var arrayType = type.GetElementType();
				if (PrimitiveFuncs.Contains(arrayType))
				{
					return GeneratePrimitiveArray(arrayType, config);
				}
				else
				{
					return GenerateCustomArray(arrayType, config);
				}
			}
			else
			{
				var ga = type.GetGenericArguments();
				var listType = ga.Any() ? ga[0] : type;
				if (PrimitiveFuncs.Contains(listType))
				{
					return GeneratePrimitiveList(listType, config);
				}
				else
				{
					return GenerateCustomList(listType, config);
				}
			}
		}

		private static IList GenerateCustomList(Type type, RandomTypeSettings config)
		{
			IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));

			var maxListSize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
			if (maxListSize > 0)
			{
				for (int i = 0; i < maxListSize; i++)
				{
					var randomModel = RandomTypeGenerator.Generate(type, config);
					list.Add(randomModel);
				}
			}

			return list;
		}

		private static IList GeneratePrimitiveList(Type type, RandomTypeSettings config)
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

		private static Array GenerateCustomArray(Type type, RandomTypeSettings config)
		{
			var maxArraySize = PrimitiveRandom.GetRandomInt32(config.Min.ListSize, config.Max.ListSize);
			Array array = Array.CreateInstance(type, maxArraySize);
			if (maxArraySize > 0)
			{
				for (int i = 0; i < maxArraySize; i++)
				{
					var randomModel = RandomTypeGenerator.Generate(type, config);
					array.SetValue(randomModel, i);
				}
			}

			return array;
		}

		private static Array GeneratePrimitiveArray(Type type, RandomTypeSettings config)
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
	}
}
