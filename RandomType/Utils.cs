using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RandomType
{
    internal static class Utils
    {
		private static Dictionary<MethodInfo, List<Func<Type, bool>>> _GetMatchesDict = null;
		public static Dictionary<MethodInfo, List<Func<Type, bool>>> GetMatchesDict
		{
			get
			{
				if (_GetMatchesDict == null)
				{
					var randomTypes = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(t => t.GetTypes())
					.Where(t => t.IsClass && Attribute.IsDefined(t, typeof(RandomTypeAttribute)));

					var funcs = new Dictionary<MethodInfo, List<Func<Type, bool>>>();
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

						funcs.Add(getMethod, matchFuncs);
					}

					_GetMatchesDict = funcs;
				}

				return _GetMatchesDict;
			}
		}
	}
}
