using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BenjcoreUtil.Versioning; 

/// <summary>
/// A simple X.Y.Z versioning system that can have as many sections as you like.
/// Can be wrapped in a <see cref="Version"/> object.
/// </summary>
public class Simple : IVersionType<object> {

  /// <summary>
  /// Dictates whether or not the current object can be compared with a <see cref="Simple"/> of a different length.
  /// </summary>
  public bool AllowDifferentLengthComparisons { get; set; } = false;
  
  /// <summary>
  /// An unsigned integer array containing the version number assigned from the constructor.
  /// </summary>
  public uint[] Data { get; }
  
  /// <summary>
  /// A property representing the length of <see cref="Data"/>.
  /// </summary>
  public int Length => Data.Length;

  /// <summary>
  /// Creates a new instance of the <see cref="Simple"/> class.
  /// </summary>
  /// <param name="values">
  /// An unsigned integer array containing the version number for the new <see cref="Simple"/> object.
  /// Each value of the array represents a different section of the version number starting with the
  /// most major version and ending with the least major. (See examples.) <see cref="Length"/> will
  /// be inferred based on the length of the array.
  /// </param>
  /// <exception cref="ArgumentException"><paramref name="values"/> is empty.</exception>
  /// <example><code>
  /// uint[] foo = new uint[] { 1, 4, 12 };
  /// Simple bar = new Simple(foo);
  /// // This would output "1.4.12".
  /// Console.WriteLine(bar.ToString());
  /// </code></example>
  public Simple(uint[] values) {

    // Ensure values has at least 1 value.
    if (values.Length < 1)
      throw new ArgumentException("Simple Version must have at least 1 value.", nameof(values));
    
    Data = values;
    
  }

  /// <summary>
  /// Compares the current object to another <see cref="Simple"/> object.
  /// </summary>
  /// <param name="input">The object you wish to compare to the current object.</param>
  /// <returns>
  /// <see cref="Dictionary{TKey,TValue}">Dictionary&lt;string, bool&gt;</see> :<br/>
  /// ["GreaterThan"] True if the current object is newer than <paramref name="input"/>, otherwise false.<br/>
  /// ["EqualTo"] True if the current object is equal to <paramref name="input"/>, otherwise false.
  /// </returns>
  /// <exception cref="VersioningException">
  /// Thrown when <paramref name="input"/> and the current object are different lengths while
  /// <see cref="AllowDifferentLengthComparisons"/> is false.
  /// </exception>
  internal Dictionary<string, bool> Calculate(Simple input) {
    
    if (!AllowDifferentLengthComparisons && Length != input.Length) {
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

  /// <summary>
  /// <inheritdoc cref="Version.isNewerThan"/>
  /// </summary>
  /// <param name="input"><inheritdoc cref="Version.isNewerThan"/></param>
  /// <returns><inheritdoc cref="Version.isNewerThan"/></returns>
  /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
  public bool isNewerThan(object input) {
    var result = Calculate((Simple)input);
    return result["GreaterThan"] && !result["EqualTo"];
  }

  /// <summary>
  /// <inheritdoc cref="Version.isNewerThanOrEqualTo"/>
  /// </summary>
  /// <param name="input"><inheritdoc cref="Version.isNewerThanOrEqualTo"/></param>
  /// <returns><inheritdoc cref="Version.isNewerThanOrEqualTo"/></returns>
  /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
  public bool isNewerThanOrEqualTo(object input) {
    var result = Calculate((Simple)input);
    return result["GreaterThan"] || result["EqualTo"];
  }

  /// <summary>
  /// <inheritdoc cref="Version.isOlderThan"/>
  /// </summary>
  /// <param name="input"><inheritdoc cref="Version.isOlderThan"/></param>
  /// <returns><inheritdoc cref="Version.isOlderThan"/></returns>
  /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
  public bool isOlderThan(object input) {
    var result = Calculate((Simple)input);
    return !result["GreaterThan"] && !result["EqualTo"];
  }

  /// <summary>
  /// <inheritdoc cref="Version.isOlderThanOrEqualTo"/>
  /// </summary>
  /// <param name="input"><inheritdoc cref="Version.isOlderThanOrEqualTo"/></param>
  /// <returns><inheritdoc cref="Version.isOlderThanOrEqualTo"/></returns>
  /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
  public bool isOlderThanOrEqualTo(object input) {
    var result = Calculate((Simple)input);
    return !result["GreaterThan"] || result["EqualTo"];
  }

  /// <summary>
  /// <inheritdoc cref="Version.IsEqualTo"/>
  /// </summary>
  /// <param name="input"><inheritdoc cref="Version.IsEqualTo"/></param>
  /// <returns><inheritdoc cref="Version.IsEqualTo"/></returns>
  /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
  public bool IsEqualTo(object input) {
    var result = Calculate((Simple)input);
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
    
    return false;

  }

  public override int GetHashCode() {
    return Data.GetHashCode();
  }

  /// <summary>
  /// Converts the string representation of a simple version number to its simple object equivalent.
  /// </summary>
  /// <param name="input">A string containing numbers separated by periods to convert. e.g. "1.2.9".</param>
  /// <returns>A simple version equivalent to the string s.</returns>
  /// <exception cref="ArgumentNullException">input is null or empty.</exception>
  /// <exception cref="FormatException">input is not in the correct format.</exception>
  /// <exception cref="OverflowException">A number in input is not within
  /// the 32-Bit signed integer limits.</exception>
  /// <remarks>input may begin with a 'v' (case insensitive) for styling.
  /// The 'v' will be ignored.</remarks>
  public static Simple Parse([NotNull] string? input) {

    if (input is null || input.Length < 1) throw new ArgumentNullException(nameof(input));

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
  /// <param name="input">A string containing numbers separated by periods to convert. e.g. "1.2.9".</param>
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