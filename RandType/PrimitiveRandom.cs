using System;
using System.Linq;
using Troschuetz.Random;

namespace RandType
{
	public class PrimitiveRandom
	{
		private static TRandom random = new TRandom();

		private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

		public static byte GetRandomByte()
		{
			var bytes = new byte[1];
			random.NextBytes(bytes);
			return bytes.First();
		}

		public static byte[] GetRandomBytes(int size)
		{
			var bytes = new byte[size];
			random.NextBytes(bytes);
			return bytes;
		}

		public static int GetRandomInt()
		{
			return random.Next();
		}

		public static int GetRandomInt(int max)
		{
			return random.Next(max);
		}

		public static int GetRandomInt(int min, int max)
		{
			return random.Next(min, max);
		}

		public static double GetRandomDouble()
		{
			return random.NextDouble();
		}

		public static double GetRandomDouble(double max)
		{
			return random.NextDouble(max);
		}

		public static double GetRandomDouble(double min, double max)
		{
			return random.NextDouble(min, max);
		}

		public static float GetRandomFloat(float min, float max)
		{
			var result = (random.NextDouble() * (max - min)) + min;
			return (float)result;
		}

		public static float GetRandomFloat(float max)
		{
			return GetRandomFloat(Single.MinValue, max);
		}

		public static float GetRandomFloat()
		{
			return GetRandomFloat(Single.MinValue, Single.MaxValue);
		}

		public static bool GetRandomBool()
		{
			return GetRandomInt(0, 2) == 1;
		}

		public static string GetRandomString(int minChars, int maxChars)
		{
			var count = GetRandomInt(minChars, maxChars);
			return GenerateString(count);
		}

		public static string GetRandomString(int maxChars)
		{
			var count = GetRandomInt(maxChars);
			return GenerateString(count);
		}

		public static string GetRandomString()
		{
			var count = GetRandomInt();
			return GenerateString(count);
		}

		/// <summary>
		/// https://stackoverflow.com/a/1344242
		/// </summary>
		/// <param name="charCount"></param>
		/// <returns></returns>
		private static string GenerateString(int charCount)
		{
			return new string(Enumerable.Repeat(chars, charCount)
				.Select(s => s[GetRandomInt(s.Length)]).ToArray());
		}

		public static char GenerateRandomChar()
		{
			return chars.ElementAt(GetRandomInt(chars.Length));
		}

		public static DateTime GetRandomDateTime()
		{
			var min = DateTime.MinValue;
			var max = DateTime.MaxValue;
			return GetRandomDateTime(min, max);
		}

		public static DateTime GetRandomDateTime(DateTime max)
		{
			var min = DateTime.MinValue;
			return GetRandomDateTime(min, max);
		}

		public static DateTime GetRandomDateTime(DateTime min, DateTime max)
		{
			var range = (max - min).TotalMilliseconds;
			var timeToAdd = GetRandomDouble(0, range);
			var randomDate = min.AddMilliseconds(timeToAdd);

			return randomDate;
		}

		public static TimeSpan GetRandomTimeSpan()
		{
			var min = TimeSpan.MinValue;
			var max = TimeSpan.MaxValue;
			return GetRandomTimeSpan(min, max);
		}

		public static TimeSpan GetRandomTimeSpan(TimeSpan max)
		{
			var min = TimeSpan.MinValue;
			return GetRandomTimeSpan(min, max);
		}

		public static TimeSpan GetRandomTimeSpan(TimeSpan min, TimeSpan max)
		{
			var range = (max - min).TotalMilliseconds;
			var timeToAdd = GetRandomDouble(0, range);
			var randomTime = min.Add(TimeSpan.FromMilliseconds(timeToAdd));

			return randomTime;
		}

		//https://stackoverflow.com/a/6651661
		public static long GetRandomInt64(long min, long max)
		{
			byte[] buf = new byte[8];
			random.NextBytes(buf);
			long longRand = BitConverter.ToInt64(buf, 0);
			return (Math.Abs(longRand % (max - min)) + min);
		}
	}
}