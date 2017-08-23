using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandType
{
	public class RandTypeSettings
	{
		public RandTypeRangeSettings Min { get; set; }

		public RandTypeRangeSettings Max { get; set; }

		public RandTypeSettings()
		{
			Min = new RandTypeRangeSettings()
			{
				Double = 0,
				Int32 = 0,
				String = 2,
				DateTime = DateTime.MinValue,
				Decimal = 0,
				ListSize = 0
			};

			Max = new RandTypeRangeSettings()
			{
				Double = 5000,
				Int32 = 5000,
				String = 30,
				DateTime = DateTime.Now,
				Decimal = 5000,
				ListSize = 5000
			};
		}
	}
}
