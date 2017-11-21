using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType.CustomRandom
{
	internal interface ICustomRandom<TR>
	{
		bool Validate(Type type);

		TR GetRandom(Type type, RandomTypeSettings settings);
	}
}
