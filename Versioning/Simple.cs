using System.Text;

namespace BenjcoreUtil.Versioning; 

public class Simple : IVersionType<object> {

  public bool AllowDifferentLengthComparisons { get; set; } = false;
  public int[] Data { get; }
  public int Length => Data.Length;

  public Simple(int[] values) {

    if (values.Length < 1) throw new NullReferenceException("Simple Version must have at least 1 value.");
    Data = values;

  }

  internal Dictionary<string, bool> Calculate(Simple input, bool allowDifferentLengths = false) {
    
    // ReSharper disable once ConvertIfStatementToSwitchStatement
    if (!allowDifferentLengths && Length != input.Length) {
      throw new VersioningException("Attempted to compare two Simple Versions of different lengths.");
    }

    Dictionary<string, bool> output = new Dictionary<string, bool> {
      ["GreaterThan"]=false,
      ["EqualTo"]=true
    };

    if (Length == input.Length) {
      
      // Check if the input is greater than current.
      for (int i = 0; i < Length; i++) {
        if (Data[i] > input.Data[i]) {
          output["GreaterThan"] = true;
          break;
        }

        if (Data[i] != input.Data[i]) {
          break;
        }
      }
      
      // Check if the input is equal to the current.
      for (int i = 0; i < Length; i++) {
        if (Data[i] != input.Data[i]) {
          output["EqualTo"] = false;
          break;
        }
      }
      
    } else if (Length > input.Length) {
      
      // Check if the input is greater than current.
      for (int i = 0; i < input.Length; i++) {
        if (Data[i] > input.Data[i]) {
          output["GreaterThan"] = true;
          break;
        }
        
        if (Data[i] != input.Data[i]) {
          break;
        }
      }

      // Check if the input is equal to the current.
      for (int i = 0; i < input.Length; i++) {
        if (Data[i] != input.Data[i]) {
          output["EqualTo"] = false;
          break;
        }
      }

      /*
       * Check for greater than as
       * well as not equal to if
       * input is longer than current
       * or vise versa AND both have
       * been equal thus far.
       */
      if (output["EqualTo"]) {
        for (int i = input.Length; i < Length; i++) {
          if (Data[i] > 0) {
            output["GreaterThan"] = true;
            output["EqualTo"] = false;
            break;
          }
        }
      }

    } else {
      
      // Check if the input is greater than current.
      for (int i = 0; i < Length; i++) {
        if (Data[i] > input.Data[i]) {
          output["GreaterThan"] = true;
          break;
        }
        
        if (Data[i] != input.Data[i]) {
          break;
        }
      }

      // Check if the input is equal to the current.
      for (int i = 0; i < Length; i++) {
        if (Data[i] != input.Data[i]) {
          output["EqualTo"] = false;
          break;
        }
      }

      /*
       * Check for greater than as
       * well as not equal to if
       * input is longer than current
       * or vise versa AND both have
       * been equal thus far.
       */
      if (output["EqualTo"]) {
        for (int i = Length; i < input.Length; i++) {
          if (input.Data[i] > 0) {
            output["GreaterThan"] = false;
            output["EqualTo"] = false;
            break;
          }
        }
      }
      
    }

    return output;

  }

  public bool isNewerThan(object input) {
    var result = Calculate((Simple)input, AllowDifferentLengthComparisons);
    return result["GreaterThan"] && !result["EqualTo"];
  }

  public bool isNewerThanOrEqualTo(object input) {
    var result = Calculate((Simple)input, AllowDifferentLengthComparisons);
    return result["GreaterThan"] || result["EqualTo"];
  }

  public bool isOlderThan(object input) {
    var result = Calculate((Simple)input, AllowDifferentLengthComparisons);
    return !result["GreaterThan"] && !result["EqualTo"];
  }

  public bool isOlderThanOrEqualTo(object input) {
    var result = Calculate((Simple)input, AllowDifferentLengthComparisons);
    return !result["GreaterThan"] || result["EqualTo"];
  }

  public bool IsEqualTo(object input) {
    var result = Calculate((Simple)input, AllowDifferentLengthComparisons);
    return result["EqualTo"];
  }

  public override string ToString() {

    StringBuilder sb = new StringBuilder();
    foreach (var item in Data) {
      sb.Append($"{item}.");
    }

    sb.Length -= 1;
    return sb.ToString();

  }
  
}

public class VersioningException : Exception {
  
  public VersioningException() { }

  public VersioningException(string Message) : base(Message) { }

  public VersioningException(string Message, Exception inner) : base(Message, inner) { }
  
}