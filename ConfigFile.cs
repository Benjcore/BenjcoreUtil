using System;
using System.IO;
using System.Collections.Generic;

namespace util {
	
    //TODO: Add Documentation to README.md

    public class ConfigFile {

        private readonly string path;
        public string Path {
            get {return path;}
        }

        private List<string> data;
        private Dictionary<int, Object> dataByInt = new Dictionary<int, Object>();
        private Dictionary<string, Object> dataByName = new Dictionary<string, Object>();
        private List<string> fullLines = new List<string>();

        public ConfigFile(string path, bool autoLoad) {
            if (autoLoad) {
                Load();
            }
            this.path = path;
        }

        public ConfigFile(string path) {
            this.path = path;
        }

        public void Load() {
            data = new List<string>(File.ReadAllLines(path));
            StreamReader sr = new StreamReader(path);
            string line;
            Object newLine;
            int count = 1;
            while ((line = sr.ReadLine()) != null) {
                if (line.StartsWith("$")) {
                    fullLines.Add(line);
                    newLine = TryParseAny(line.Split('=')[1]);
                    dataByInt.Add(count, newLine);
                    count++;
                }
            }
            for (int c = 1; c <= dataByInt.Count; c++) {
                dataByName.Add((fullLines[c - 1]).Split('=')[0].Split('$')[1], dataByInt[c]);
            }
        }

        public Object Get(int index) {
            return dataByInt[index];
        }

        public Object Get(string name) {
            return dataByName[name];
        }

        public Object GetType(int index) {
            return dataByInt[index].GetType();
        }

        public Object GetType(string name) {
            return dataByName[name].GetType();
        }

        private static Object TryParseAny(string input) {
            try {
            if (Boolean.TryParse(input, out bool asBool)) {
                    return asBool;
                } else if (Char.TryParse(input.Remove(0, 1).Remove(1, 1), out char asChar)) {
                    return asChar;
                }
            } catch (ArgumentOutOfRangeException ignored) {
                //This just stops VSCode from saying there is a warning. :)
                string dump = ignored.StackTrace;
            }
            if (Int32.TryParse(input, out int asInt)) {
                return asInt;
            } else if (Int64.TryParse(input, out long asLong)) {
                return asLong;
            } else if (Double.TryParse(input, out double asDouble)) {
                return asDouble;
            } else if (input.StartsWith("\"") & input.EndsWith("\"")) {
                return (string)input.Remove(0, 1).Remove(input.Length - 2, 1);
            } else {
                throw new InvalidCastException("The Input was not a valid bool, char, double, int, long or string.");
            }
        }

    }

}
