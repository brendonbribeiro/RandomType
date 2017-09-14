using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RandomType
{
	public class PrimitiveRandom
	{
		//private static TRandom random = new TRandom();

		private static RandomNumberGenerator rng = RandomNumberGenerator.Create();

		public static Random random = new Random();

		private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ";

		public static byte GetRandomByte()
		{
			var bytes = new byte[1];
			rng.GetBytes(bytes);
			return bytes.First();
		}

		public static byte[] GetRandomBytes(int size)
		{
			var bytes = new byte[size];
			rng.GetBytes(bytes);
			return bytes;
		}

		////http://www.vcskicks.com/code-snippet/rng-int.php
		private static Int32 GenerateSeed(int size)
		{
			byte[] buffer = new byte[size];
			rng.GetBytes(buffer);
			Int32 result = BitConverter.ToInt32(buffer, 0);

			return result;
		}


		public static Int32 GetRandomInt32(Int32 min, Int32 max)
		{
			return random.Next(min, max);
			//return new Random(GenerateSeed(sizeof(Int32))).Next(min, max);
		}

		public static Int32 GetRandomInt32()
		{
			return random.Next();
			//return new Random(GenerateSeed(sizeof(Int32))).Next();
		}

		public static Double GetRandomDouble()
		{
			//var rDouble = new Random(GenerateSeed(sizeof(Double))).NextDouble() * GetRandomInt32();
			var rDouble = random.NextDouble() * GetRandomInt32();
			return rDouble;
		}

		public static Single GetRandomFloat()
		{
			byte[] buffer = new byte[sizeof(Single)];
			rng.GetBytes(buffer);
			Single result = BitConverter.ToSingle(buffer, 0);

			return result;
		}

		public static bool GetRandomBool()
		{
			return GetRandomInt32(0, 2) == 1;
		}

		public static string GetRandomString(int maxChars)
		{
			var count = GetRandomInt32(0, maxChars);
			return GenerateString(count);
		}

		public static string GetRandomString(int minChars, int maxChars)
		{
			var count = GetRandomInt32(minChars, maxChars);
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
				.Select(s => s[GetRandomInt32(0, s.Length)]).ToArray());
		}

		public static char GenerateRandomChar()
		{
			return chars.ElementAt(GetRandomInt32(0, chars.Length));
		}

		public static DateTime GetRandomDateTime()
		{
			DateTime start = DateTime.MinValue;
			int range = (DateTime.MaxValue - start).Days;
			return start.AddDays(random.Next(range));
		}

		public static TimeSpan GetRandomTimeSpan()
		{
			return new TimeSpan(GetRandomInt64());
		}

		public static long GetRandomInt64()
		{
			byte[] buffer = new byte[sizeof(Int64)];
			rng.GetBytes(buffer);
			Int64 result = BitConverter.ToInt64(buffer, 0);

			return result;
		}
	}
}
