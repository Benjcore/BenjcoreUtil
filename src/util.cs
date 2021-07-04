using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;

namespace util
{

	public static class util {
		
		public static void clear() {
			Console.WriteLine("================================");
			Process.Start("cmd.exe", "/c cls").WaitForExit();
		}

		public static Dictionary<string, int> SwapDictionary(Dictionary<int, string> input) {
			/*
			Converts Dictionary<int, string>
			to Dictionary<string, int>
			assuming the input starts at 0 and goes up by one.
			*/
			Dictionary<string, int> output = new Dictionary<string, int>();
			foreach (KeyValuePair<int, string> item in input) {
				output.Add(item.Value, item.Key);
			}
			return output;
		}

	}

	public class DateObject {

		private string date;
		public string Date {
			get {
				return date;
			}
			protected set {
				date = value;
			}
		}

		private bool dmy;
		public bool Dmy {
			get {
				return dmy;
			}
			protected set {
				dmy = value;
			}
		}

		private bool compact;
		public bool Compact {
			get {
				return compact;
			}
			protected set {
				compact = value;
			}
		}

		private bool fileNameFriendly;
		public bool FileNameFriendly {
			get {
				return fileNameFriendly;
			}
			protected set {
				fileNameFriendly = value;
			}
		}

		public DateObject(bool dmy, bool compact, bool fileNameFriendly) {
			this.Dmy = dmy;
			this.Compact = compact;
			this.FileNameFriendly = fileNameFriendly;
			Date = getDate(dmy, compact, fileNameFriendly);
		}

		public void update() {
			Date = getDate(Dmy, Compact, FileNameFriendly);
		}

		public string GetDate() {
			update();
			return Date;
		}

		public string GetDate(bool dontUpdate) {
			if (!dontUpdate) {
				update();
			}
			return Date;
		}

		public static string getDate(bool dmy, bool compact, bool FileNameFriendly) {
			//Returns The Current Date.
			DateTime date = new DateTime();
			char symbol;
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

	}

	public class TimeObject {

		private string time;
		public string Time {
			get {
				return time;
			}
			protected set {
				time = value;
			}
		}

		private bool twentyfourhour;
		public bool Twentyfourhour {
			get {
				return twentyfourhour;
			}
			protected set {
				twentyfourhour = value;
			}
		}

		private bool ms;
		public bool Ms {
			get {
				return ms;
			}
			protected set {
				ms = value;
			}
		}

		private bool fileNameFriendly;
		public bool FileNameFriendly {
			get {
				return fileNameFriendly;
			}
			protected set {
				fileNameFriendly = value;
			}
		}

		public TimeObject(bool twentyfourhour, bool ms, bool FileNameFriendly) {
			this.Twentyfourhour = twentyfourhour;
			this.Ms = ms;
			this.FileNameFriendly = fileNameFriendly;
			Time = getTime(twentyfourhour, ms, fileNameFriendly);
		}

		public void update() {
			Time = getTime(Twentyfourhour, Ms, FileNameFriendly);
		}

		public string GetTime() {
			update();
			return Time;
		}

		public string GetTime(bool dontUpdate) {
			if (!dontUpdate) {
				update();
			}
			return Time;
		}

		public static string getTime(bool twentyfourhour, bool ms, bool FileNameFriendly) {
			//Returns The Current Time.
			DateTime date = new DateTime();
			char symbol;
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
