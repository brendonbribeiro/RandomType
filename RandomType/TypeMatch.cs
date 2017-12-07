using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RandomType
{
    internal class TypeMatch
    {
		public MethodInfo GetMethod { get; set; }

		public List<Func<Type, bool>> MatchFuncs { get; set; }
	}
}
