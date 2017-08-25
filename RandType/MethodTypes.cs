using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RandType
{
	internal class MethodTypes
	{
		public List<Type> Types { get; set; }

		public MethodInfo Method { get; set; }

		public MethodTypes(MethodInfo method, params Type[] types)
		{
			Method = method;
			Types = types.ToList();
		}
	}
}