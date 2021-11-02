using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace util {

    public class LogFile : IFileObject {

        private readonly string path;
        public string Path {
            get {
                return path;
            }
        }

        private List<Logger> dependents = new List<Logger>();
        public List<Logger> Dependents {
            get {
                return dependents;
            }
            protected set {
                dependents.Add(value[0]);
            }
        }

        public LogFile(string path) {
            this.path = path;
        }

    }

    public class Logger : LogFile {

        //Variables
        private readonly string loggerName;
        private readonly LogFile logFile;
        private readonly string[] levels;
        private readonly bool showDate;
        private readonly bool showTime;
        private readonly bool showName;
        private readonly bool forceLevelToStart;
        private readonly bool useDMY;
        private readonly bool compactDate;
        private readonly bool includeMS;
        private readonly bool use24hTime;
        private readonly string printReq;
        private readonly string logReq;
        private DateObject date;
        private TimeObject time;

        //Properties
        public string LoggerName {get{return loggerName;}}
        public LogFile LogFile {get{return logFile;}}
        public string[] Levels {get{return levels;}}
        public bool ShowDate {get{return showDate;}}
        public bool ShowTime {get{return showTime;}}
        public bool ShowName {get{return showName;}}
        public bool ForceLevelToStart {get{return forceLevelToStart;}}
        public bool UseDMY {get{return useDMY;}}
        public bool CompactDate {get{return compactDate;}}
        public bool IncludeMS {get{return includeMS;}}
        public bool Use24hTime {get{return use24hTime;}}
        public string PrintReq {get{return printReq;}}
        public string LogReq {get{return logReq;}}
        private DateObject Date {get{return date;}}
        private TimeObject Time {get{return time;}}

        //DEBUG
        private readonly bool debugMode = false;

        //Constructor
        public Logger(
            string loggerName,
            LogFile logFile,
            string[] levels,
            string logReq,
            string printReq = "None",
            bool showDate = false,
            bool showTime = false,
            bool showName = false,
            bool forceLevelToStart = false,
            bool useDMY = true,
            bool compactDate = false,
            bool includeMS = false,
            bool use24hTime = true
        ) : base(logFile.Path) {
            //TODO: Add Documentation to README.md

            //Set Levels to UpperCase
            for (int c = 0; c < levels.Length; c++) {
                levels[c] = levels[c].ToUpper();
            }

            //Check if printReq and logReq are valid
            if (!(levels.Contains(logReq.ToUpper()) || logReq == "All" || logReq == "None")) {
                throw new Exceptions.UnkownLoggerLevelException($"Could not find logReq level '{logReq.ToUpper()}'.");
            } else if (!(levels.Contains(printReq.ToUpper()) || printReq == "All" || printReq == "None")) {
                throw new Exceptions.UnkownLoggerLevelException($"Could not find printReq level '{printReq.ToUpper()}'.");
            }

            //Initialize Variables
            this.loggerName = loggerName;
            this.logFile = logFile;
            this.levels = levels;
            this.printReq = printReq.ToUpper();
            this.logReq = logReq.ToUpper();
            this.showDate = showDate;
            this.showTime = showTime;
            this.showName = showName;
            this.forceLevelToStart = forceLevelToStart;
            this.useDMY = useDMY;
            this.compactDate = compactDate;
            this.includeMS = includeMS;
            this.use24hTime = use24hTime;

            //Initialize Date & Time Objects
            if (showDate) {
                this.date = new DateObject(useDMY, compactDate, false);
            }
            if (showTime) {
                this.time = new TimeObject(use24hTime, includeMS, false);
            }

            //Removes log file if it exists.
            if (io.exist(LogFile.Path)) {
                io.del(LogFile.Path);
            }

            if (debugMode) {
                Console.WriteLine("[WARNING] util Logger running in debug mode!");
            }
        }

        public void Log(string level, string msg) {
            //Logs info and errors.
            //====================
            //Check Logging File
            string path = LogFile.Path;
            bool newFile = false;
            bool logToFile = false;
            bool print = false;
            level = level.ToUpper();
            if (!io.exist(path)) {
                newFile = true;
            }
            //Check if input level is valid
            if (!Levels.Contains(level)) {
                throw new Exceptions.UnkownLoggerLevelException($"'{level} doesn't seem to be a valid Level.'");
            }
            //Map Levels to Numbers
            //The Lower the Number the more Severe the alert
            Dictionary<int, string> levelMap = new Dictionary<int, string>();
            int count = 0;
            foreach (string thisLevel in this.Levels) {
                levelMap.Add(count, thisLevel);
                count++;
            }
            Dictionary<string, int> levelMapReverse = new Dictionary<string, int>(util.SwapDictionary(levelMap));
            //Check to see if the current level meets the Log Req.
            if (LogReq == "ALL") {
                logToFile = true;
            } else if (LogReq == "NONE") {
                logToFile = false;
            } else {
                if (levelMapReverse[level] <= levelMapReverse[LogReq]) logToFile = true;
            }
            //Check to see if the current level meets the Print Req.
            if (PrintReq == "ALL") {
                print = true;
            } else if (PrintReq == "NONE") {
                print = false;
            } else {
                if (levelMapReverse[level] <= levelMapReverse[PrintReq]) print = true;
            }
            //Update Time & Date
            date.update();
            time.update();
            //Generate Output
            string output = "";
            if (logToFile || print) {
                if (showName) {
                    output += $"[{LoggerName}] ";
                }
                if (ForceLevelToStart) {
                    output += $"[{level}] ";
                }
                if (ShowDate) {
                    output += $"{Date.Date} ";
                }
                if (showTime) {
                    output += $"{Time.Time} ";
                }
                if (!ForceLevelToStart) {
                    output += $"[{level}] ";
                }
                output += ": " + msg;

                //Print and Log the Output
                if (print) {
                    //Print Output
                    Console.WriteLine(output);
                }
                if (logToFile) {
                    //Log Output to File
                    if (newFile) {
                        StreamWriter sw = new StreamWriter(LogFile.Path);
                        sw.WriteLine(output);
                        sw.Close();
                    } else {
                        StreamWriter sw = new StreamWriter(LogFile.Path, true);
                        sw.WriteLine(output);
                        sw.Close();
                    }
                }
            }
            if (debugMode) {
                Console.WriteLine($"L : {level} PR : {print} LR : {logToFile}");
            }
        }

    }

}
