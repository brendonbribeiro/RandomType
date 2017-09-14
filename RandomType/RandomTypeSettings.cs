using System;
using System.Collections.Generic;
using System.Text;

namespace RandomType
{
	public class RandomTypeSettings
	{
		public RandomTypeRangeSettings Min { get; set; }

		public RandomTypeRangeSettings Max { get; set; }

		public RandomTypeSettings()
		{
			Min = new RandomTypeRangeSettings()
			{
				//Double = 0,
				//Float = Single.MinValue,
				Int32 = Int32.MinValue,
				//Int64 = Int64.MinValue,
				String = 0,
				DateTime = DateTime.MinValue,
				//Decimal = Decimal.MinValue,
				ListSize = 0,
				TimeSpan = new TimeSpan()
			};

			Max = new RandomTypeRangeSettings()
			{
				//Double = Double.MaxValue,
				//Float = Single.MaxValue,
				Int32 = Int32.MaxValue,
				//Int64 = Int64.MaxValue,
				String = 50,
				DateTime = DateTime.Now,
				//Decimal = Decimal.MaxValue,
				ListSize = 50,
				TimeSpan = TimeSpan.MaxValue
			};
		}
	}
}
