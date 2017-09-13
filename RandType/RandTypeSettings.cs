using System;

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

			Max = new RandTypeRangeSettings()
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