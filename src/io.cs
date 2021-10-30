using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace util {

    public static class io {

        /*
        IMPORTANT :

        THE READ / WRITE METHODS OF THE IO CLASS
        WERE REMOVED IN THE C# PORT OF UTIL
        DUE TO OFFICIAL METHODS BEING VERY
        SIMPLE TO USE FOR QUICK TASKS UNLIKE JAVA.
        */

        public static string dir() {
            //Returns The Current Directory
            //Eg. C:\\Users\\Benj_2005\\Documents
            return Directory.GetCurrentDirectory();
        }

        public static void makeDir(string Name) {
            //Creates a Directory
            Directory.CreateDirectory(Name);
        }

        public static void removeDir(string Name, bool ForceRemove) {
            //Deletes a Directory
            //If ForceRemove = true it will also delete the contents
            if (!ForceRemove) {
                Directory.Delete(Name);
            } else {
                Process.Start("cmd.exe", "/c rd /s /q " + Name).WaitForExit();
            }
        }

        public static void removeDir(string Name) {
            //Deletes a Directory
            Directory.Delete(Name);
        }

        public static bool dirExist(string Name) {
            //Checks if a Directory exists
            return Directory.Exists(Name);
        }

        public static void cd(string Name) {
            //Changes Current Working Directory
            Directory.SetCurrentDirectory(Name);
        }

        public static void newFile(string Name) {
            //Creates a new file
            if (!File.Exists(Name)) {
                File.Create(Name);
            } else {
                throw new IOException($"File {Name} already exists.");
            }
        }

        public static bool exist(string Name) {
            //Checks if a file exists
            return File.Exists(Name);
        }

        public static bool testDir(string testFile, bool StackTrace) {
            //Checks if the program can read and write to a directory with a sample file.
            const string sampleText = "This is a Test file.";
            try {
            if (exist(testFile)) throw new IOException($"{testFile} already exists.");
            } catch (IOException e) {
                if (StackTrace) {
                    Console.WriteLine(e.StackTrace);
                }
                return false;
            }
            try {
                StreamWriter sw = new StreamWriter(testFile, false);
                sw.Write(sampleText);
                sw.Close();
                if (!exist(testFile)) throw new IOException("Test Failed");
                if (!(File.ReadAllLines(testFile)[0] == sampleText)) throw new IOException("Test Failed");
                del(testFile);
                if (exist(testFile)) throw new IOException("Test Failed");
                return true;
            } catch (Exception e) {
                if (StackTrace) {
                    Console.WriteLine(e.StackTrace);
                }
                return false;
            }
        }

        public static void wipe(string Name) {
            //Wipe the contents of a file
            StreamWriter sw = new StreamWriter(Name, false);
            sw.Write("");
            sw.Close();
        }

        public static int getLines(string Name) {
            //Returns the amount of lines in a file
            return File.ReadAllLines(Name).Length;
        }

        public static void del(string Name) {
            //Deletes a file
            File.Delete(Name);
        }

        /*
        IMPORTANT :

        THE READ / WRITE METHODS OF THE IO CLASS
        WERE REMOVED IN THE C# PORT OF UTIL
        DUE TO OFFICIAL METHODS BEING VERY
        SIMPLE TO USE FOR QUICK TASKS UNLIKE JAVA.
        */

    }

}