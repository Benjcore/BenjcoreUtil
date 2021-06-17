using System;
using System.Diagnostics;
using System.Globalization;

namespace util
{

	public class util {

		private static char symbol;
		
		public static void clear() {
			Console.WriteLine("================================");
			Process.Start("cmd.exe", "/c cls").WaitForExit();
		}

		public static string getDate(bool dmy, bool compact, bool FileNameFriendly) {
			//Returns The Current Date.
			DateTime date = new DateTime();
			date = DateTime.Now;
			string format;
			if (FileNameFriendly) {
				symbol = '-';
			} else {
				symbol = '/';
			}
			if (dmy) {
				format = $"dd{symbol}MM{symbol}";
			} else {
				format = $"MM{symbol}dd{symbol}";
			}
			if (compact) {
				format = format + "y";
			} else {
				format = format + "yyyy";
			}
			return date.ToString(format);
		}

		public static string getTime(bool twentyfourhour, bool ms, bool FileNameFriendly) {
			//Returns The Current Time.
			DateTime date = new DateTime();
			date = DateTime.Now;
			string format;
			if (FileNameFriendly) {
				symbol = '-';
			} else {
				symbol = ':';
			}
			if (twentyfourhour) {
				format = $"HH{symbol}mm{symbol}ss";
			} else {
				format = $"hh{symbol}mm{symbol}ss";
			}
			if (ms) {
				format = format + $"{symbol}ffff";
			}
			if (!twentyfourhour) {
				format = format + " tt";
			}
			return date.ToString(format);
		}

	}

}
