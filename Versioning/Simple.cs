using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace BenjcoreUtil.Versioning; 

public class Simple : IVersionType<object> {

  public bool AllowDifferentLengthComparisons { get; set; } = false;
  public uint[] Data { get; }
  public int Length => Data.Length;

  public Simple(uint[] values) {

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
    
    int len = Math.Max(Length, input.Length);
    
    // EqualTo
    for (int i = 0; i < len; i++) {

      if (i >= Data.Length) {
        
        if (input.Data[i] != 0) {
          output["EqualTo"] = false;
          break;
        }
        
      } else if (i >= input.Data.Length) {
        
        if (Data[i] != 0) {
          output["EqualTo"] = false;
          break;
        }
        
      } else {
        
        if (Data[i] != input.Data[i]) {
          output["EqualTo"] = false;
          break;
        }
        
      }
      
    }
    
    // GreaterThan
    for (int i = 0; i < len; i++) {

      if (i >= Data.Length) {
        
        if (input.Data[i] != 0) {
          output["GreaterThan"] = false;
          break;
        }
        
      } else if (i >= input.Data.Length) {
        
        if (Data[i] != 0) {
          output["GreaterThan"] = true;
          break;
        }
        
      } else {
        
        if (Data[i] != input.Data[i]) {
          output["GreaterThan"] = Data[i] > input.Data[i];
          break;
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
  
  // Operators
  public static explicit operator string(Simple input) => input.ToString();

  public static bool operator ==(Simple? x, Simple? y) {
    if ((object?)y == null) return (object?)x == null;
    if ((object?)x == null) return false;
    return x.IsEqualTo(y);
  }
  public static bool operator !=(Simple? x, Simple? y) { return !(x == y); }
  public static bool operator >(Simple x, Simple y) { return x.isNewerThan(y); }
  public static bool operator <(Simple x, Simple y) { return x.isOlderThan(y); }
  public static bool operator >=(Simple x, Simple y) { return x.isNewerThanOrEqualTo(y); }
  public static bool operator <=(Simple x, Simple y) { return x.isOlderThanOrEqualTo(y); }

  public override string ToString() {

    StringBuilder sb = new StringBuilder();
    foreach (var item in Data) {
      sb.Append($"{item}.");
    }

    sb.Length -= 1;
    return sb.ToString();

  }

  public override bool Equals(object? obj) {

    if (obj is Simple) {
      return IsEqualTo(obj);
    }
    
    return RuntimeHelpers.Equals(this, obj);

  }

  public override int GetHashCode() {
    return Data.GetHashCode();
  }

  /// <summary>
  /// Converts the string representation of a simple version number to its simple object equivalent.
  /// </summary>
  /// <param name="input">A string containing numbers seperated by periods to convert. e.g. "1.2.9".</param>
  /// <returns>A simple version equivalent to the string s.</returns>
  /// <exception cref="ArgumentNullException">input is null.</exception>
  /// <exception cref="FormatException">input is not in the correct format.</exception>
  /// <exception cref="OverflowException">A number in input is not within
  /// the 32-Bit signed integer limits.</exception>
  /// <remarks>input may begin with a 'v' (case insensitive) for styling.
  /// The 'v' will be ignored.</remarks>
  public static Simple Parse([NotNull] string? input) {

    if (input == null) throw new ArgumentNullException(nameof(input));

    if (input.ToLower()[0] is 'v') input = input[1..];
    string[] inputSplit = input.Split('.');
    int len = inputSplit.Length;
    
    uint[] data = new uint[len];
    for (int i = 0; i < len; i++) { data[i] = UInt32.Parse(inputSplit[i]); }
    return new Simple(data);

  }

  /// <summary>
  /// Converts the string representation of a simple version number to its simple version object equivalent.
  /// <br></br>A return value indicates whether the conversion succeeded.
  /// </summary>
  /// <param name="input">A string containing numbers seperated by periods to convert. e.g. "1.2.9".</param>
  /// <param name="result">When this method returns, contains the simple version object equivalent of
  /// the string input, if the conversion succeeded, or null if the conversion failed.</param>
  /// <returns>true if input was converted successfully; otherwise, false.</returns>
  /// <remarks>input may begin with a 'v' (case insensitive) for styling. The 'v' will be ignored.</remarks>
  public static bool TryParse([NotNullWhen(true)] string? input, out Simple? result) {

    try {
      result = Parse(input);
      return true;
    } catch (Exception e) when (
      e is ArgumentNullException or FormatException or OverflowException
    ) {
      result = null;
      return false;
    }

  }
  
}