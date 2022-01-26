using System.Diagnostics;

namespace BenjcoreUtil; 

public static class Misc {

  // Clears The Window of a Console App.
  public static void Clear(bool useCmd = false) {
    
    /*
     * Writes blank space to the console
     * in case the clearing method fails.
     */
    Console.WriteLine(new string('\n', 128));
    switch (useCmd) {
      
      // Use Console.Clear()
      case false:
        try {
          Console.Clear();
        } catch {
          /* ignored */
        }
        return;
      
      // Use CMD 'cls' command.
      case true:
        Process.Start("cmd.exe", "/c cls").WaitForExit();
        return;
      
    }

  }
  
}