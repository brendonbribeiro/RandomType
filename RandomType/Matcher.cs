using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RandomType
{
	/// <summary>
	/// Search for the implementation of the informed type
	/// </summary>
	internal static class Matcher
	{
		private static List<TypeMatch> _TypeMatches = null;

		/// <summary>
		/// List of available types to generate (based on 'RandomType.Types')
		/// </summary>
		private static List<TypeMatch> TypeMatches
		{
			get
			{
				if (_TypeMatches == null)
				{
					var randomTypes = AppDomain.CurrentDomain.GetAssemblies()
						.SelectMany(t => t.GetTypes())
						.Where(t => t.IsClass && Attribute.IsDefined(t, typeof(RandomTypeAttribute)));

					var typeMatches = new List<TypeMatch>();
					foreach (var randomType in randomTypes)
					{
						var matchMethods = randomType.GetMethods(BindingFlags.Static | BindingFlags.Public).Where(t => Attribute.IsDefined(t, typeof(MatchAttribute)));
						var matchFuncs = new List<Func<Type, bool>>();
						foreach (var matchMethod in matchMethods)
						{
							Func<Type, bool> matchFunc = (Func<Type, bool>)
											Delegate.CreateDelegate(typeof(Func<Type, bool>), matchMethod);
							matchFuncs.Add(matchFunc);
						}

						var getMethod = randomType.GetMethods(BindingFlags.Static | BindingFlags.Public).Where(t => Attribute.IsDefined(t, typeof(GetAttribute))).First();

						typeMatches.Add(new TypeMatch()
						{
							GetMethod = getMethod,
							MatchFuncs = matchFuncs
						});
					}

					_TypeMatches = typeMatches;
				}

				return _TypeMatches;
			}
		}

		/// <summary>
		/// Searchs for a type match, and if it succeed generates the random value
		/// </summary>
		/// <param name="type">The type to be matched</param>
		/// <param name="configuration">User configuration</param>
		/// <returns>Key: if it succeeded, Value: random value (in case of error, it will be null)</returns>
		public static KeyValuePair<bool, object> TryMatch(Type type, RandomTypeSettings configuration)
		{
			var matchResult = TypeMatches.FirstOrDefault(gm => gm.MatchFuncs.All(a => a.Invoke(type)));
			if (matchResult != null)
			{
				var value = matchResult.GetMethod.Invoke(null, new object[] { type, configuration });
				return new KeyValuePair<bool, object>(true, value);
			}

			return new KeyValuePair<bool, object>(false, null);
		}


	}
}
