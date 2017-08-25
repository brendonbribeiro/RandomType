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
				Double = 0,
				Float = 0,
				Int32 = 0,
				Int64 = 0,
				String = 0,
				DateTime = DateTime.MinValue,
				Decimal = 0,
				ListSize = 0,
				TimeSpan = new TimeSpan()
			};

			Max = new RandTypeRangeSettings()
			{
				Double = Double.MaxValue,
				Float = Single.MaxValue,
				Int32 = Int32.MaxValue,
				Int64 = Int64.MaxValue,
				String = 50,
				DateTime = DateTime.Now,
				Decimal = Decimal.MaxValue,
				ListSize = 50,
				TimeSpan = TimeSpan.MaxValue
			};
		}
	}
}