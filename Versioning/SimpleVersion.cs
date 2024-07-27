using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace BenjcoreUtil.Versioning;

/// <summary>
/// A SimpleVersion X.Y.Z versioning system that can have as many sections as you like.
/// </summary>
public class SimpleVersion : IParseableVersion<SimpleVersion>, IVersion
{
    /// <summary>
    /// Dictates whether or not the current object can be compared with a <see cref="SimpleVersion"/> of a different length.
    /// </summary>
    public bool AllowDifferentLengthComparisons { get; set; } = false;
    
    /// <summary>
    /// An unsigned integer array containing the version number assigned from the constructor.
    /// </summary>
    public uint[] Data { get; private init; }
    
    /// <summary>
    /// A property representing the length of <see cref="Data"/>.
    /// </summary>
    public int Length => Data.Length;
    
    /// <summary>
    /// Creates a new instance of the <see cref="SimpleVersion"/> class.
    /// </summary>
    /// <param name="values">
    /// An unsigned integer array containing the version number for the new <see cref="SimpleVersion"/> object.
    /// Each value of the array represents a different section of the version number starting with the
    /// most major version and ending with the least major. (See examples.) <see cref="Length"/> will
    /// be inferred based on the length of the array.
    /// </param>
    /// <exception cref="ArgumentException"><paramref name="values"/> is empty.</exception>
    /// <example><code>
    /// uint[] foo = new uint[] { 1, 4, 12 };
    /// SimpleVersion bar = new SimpleVersion(foo);
    /// // This would output "1.4.12".
    /// Console.WriteLine(bar.ToString());
    /// </code></example>
    public SimpleVersion(uint[] values)
    {
        // Ensure values has at least 1 value.
        if (values.Length < 1)
        {
            throw new ArgumentException("SimpleVersion Version must have at least 1 value.", nameof(values));
        }
        
        Data = values;
    }
    
    /// <summary>
    /// Compares the current object to another <see cref="SimpleVersion"/> object.
    /// </summary>
    /// <param name="input">The object you wish to compare to the current object.</param>
    /// <returns>
    /// A tuple where the first bool indicates whether or not the current instance is newer than the other,
    /// and the second bool indicates whether or not the current instance is equal to the other.
    /// </returns>
    /// <exception cref="VersioningException">
    /// Thrown when <paramref name="input"/> and the current object are different lengths while
    /// <see cref="AllowDifferentLengthComparisons"/> is false.
    /// </exception>
    internal (bool NewerThan, bool EqualTo) Calculate(SimpleVersion input)
    {
        SimpleVersion current = new(Data);
        
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        // ^ It looks quite messy to use a switch statement for this.
        if (!AllowDifferentLengthComparisons && current.Length != input.Length)
        {
            throw new VersioningException("Attempted to compare two SimpleVersions of different lengths.");
        }
        
        if (AllowDifferentLengthComparisons && current.Length != input.Length)
        {
            /*
             * Append zeros to the end of the shorter version until
             * it's length is equal to that of the longer version.
             */
            
            // Get the shorter of two versions.
            SimpleVersion shorter = current.Length < input.Length ? current : input;
            
            // Get the longer of two versions.
            SimpleVersion longer = current.Length > input.Length ? current : input;
            
            List<uint> newData = shorter.Data.ToList();
            
            for (int i = 0; i < longer.Length - shorter.Length; i++)
            {
                newData.Add(0);
            }
            
            // Set the shorter version's data to the new data.
            // Note: We have to check which version is shorter
            // again because we can't dereference 'shorter'
            // without using unsafe pointers.
            if (current.Length < input.Length)
            {
                current = new SimpleVersion(newData.ToArray());
            }
            else
            {
                input = new SimpleVersion(newData.ToArray());
            }
        }
        
        bool greaterThan = false;
        bool equalTo = true;
        
        // Note: Both versions are the same length at this point.
        int len = current.Length;
        
        // Compare each value in the current version to determine if
        // it's greater than the input and if it's equal to the input.
        for (int i = 0; i < len; i++)
        {
            if (current.Data[i] != input.Data[i])
            {
                greaterThan = current.Data[i] > input.Data[i];
                equalTo = false;
                break;
            }
        }
        
        return (greaterThan, equalTo);
    }
    
    /// <summary>
    /// <inheritdoc cref="IVersion.IsNewerThan"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="IVersion.IsNewerThan"/></param>
    /// <returns><inheritdoc cref="IVersion.IsNewerThan"/></returns>
    /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="input"/> is not of type <see cref="SimpleVersion"/>.
    /// </exception>
    public bool IsNewerThan(IVersion input)
    {
        if (input is not SimpleVersion version)
        {
            throw new InvalidOperationException("SimpleVersion.isNewerThan can only be used with SimpleVersion objects.");
        }
        
        var result = Calculate(version);
        return result.NewerThan;
    }
    
    /// <summary>
    /// <inheritdoc cref="IVersion.IsNewerThanOrEqualTo"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="IVersion.IsNewerThanOrEqualTo"/></param>
    /// <returns><inheritdoc cref="IVersion.IsNewerThanOrEqualTo"/></returns>
    /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="input"/> is not of type <see cref="SimpleVersion"/>.
    /// </exception>
    public bool IsNewerThanOrEqualTo(IVersion input)
    {
        if (input is not SimpleVersion version)
        {
            throw new InvalidOperationException("SimpleVersion.isNewerThanOrEqualTo can only be used with SimpleVersion objects.");
        }
        
        var result = Calculate(version);
        return result.NewerThan || result.EqualTo;
    }
    
    /// <summary>
    /// <inheritdoc cref="IVersion.IsOlderThan"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="IVersion.IsOlderThan"/></param>
    /// <returns><inheritdoc cref="IVersion.IsOlderThan"/></returns>
    /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="input"/> is not of type <see cref="SimpleVersion"/>.
    /// </exception>
    public bool IsOlderThan(IVersion input)
    {
        if (input is not SimpleVersion version)
        {
            throw new InvalidOperationException("SimpleVersion.isOlderThan can only be used with SimpleVersion objects.");
        }
        
        var result = Calculate(version);
        return result is { NewerThan: false, EqualTo: false };
    }
    
    /// <summary>
    /// <inheritdoc cref="IVersion.IsOlderThanOrEqualTo"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="IVersion.IsOlderThanOrEqualTo"/></param>
    /// <returns><inheritdoc cref="IVersion.IsOlderThanOrEqualTo"/></returns>
    /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="input"/> is not of type <see cref="SimpleVersion"/>.
    /// </exception>
    public bool IsOlderThanOrEqualTo(IVersion input)
    {
        if (input is not SimpleVersion version)
        {
            throw new InvalidOperationException("SimpleVersion.isOlderThanOrEqualTo can only be used with SimpleVersion objects.");
        }
        
        var result = Calculate(version);
        return !result.NewerThan || result.EqualTo;
    }
    
    /// <summary>
    /// <inheritdoc cref="IVersion.IsEqualTo"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="IVersion.IsEqualTo"/></param>
    /// <returns><inheritdoc cref="IVersion.IsEqualTo"/></returns>
    /// <exception cref="VersioningException"><inheritdoc cref="Calculate"/></exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="input"/> is not of type <see cref="SimpleVersion"/>.
    /// </exception>
    public bool IsEqualTo(IVersion input)
    {
        if (input is not SimpleVersion version)
        {
            throw new InvalidOperationException("SimpleVersion.IsEqualTo can only be used with SimpleVersion objects.");
        }
        
        var result = Calculate(version);
        return result.EqualTo;
    }
    
#region Operators
    public static explicit operator string(SimpleVersion input) => input.ToString();
    
    public static bool operator ==(SimpleVersion? x, SimpleVersion? y)
    {
        if ((object?) y is null)
            return (object?) x is null;
        
        if ((object?) x is null)
            return false;
        
        return x.IsEqualTo(y);
    }
    
    public static bool operator !=(SimpleVersion? x, SimpleVersion? y) => !(x == y);
    public static bool operator >(SimpleVersion x, SimpleVersion y) => x.IsNewerThan(y);
    public static bool operator <(SimpleVersion x, SimpleVersion y) => x.IsOlderThan(y);
    public static bool operator >=(SimpleVersion x, SimpleVersion y) => x.IsNewerThanOrEqualTo(y);
    public static bool operator <=(SimpleVersion x, SimpleVersion y) => x.IsOlderThanOrEqualTo(y);
#endregion
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        
        foreach (var item in Data)
        {
            sb.Append($"{item}.");
        }
        
        sb.Length -= 1;
        return sb.ToString();
    }
    
    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    /// <remarks>
    /// <b>NOTE: This method does NOT compare references like object.Equals().</b>
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (obj is SimpleVersion version)
        {
            return IsEqualTo(version);
        }
        
        return false;
    }
    
    /// <summary>
    /// Gets the hash code for the current object.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    /// <remarks>
    /// <b>NOTE: This method does NOT return the hash code of the reference like object.GetHashCode().</b>
    /// </remarks>
    public override int GetHashCode()
    {
        /*
         * This hash code implementation basically creates a
         * comma-separated string of the version data and then
         * returns the hash code of that string.
         */
        
        StringBuilder sb = new StringBuilder();
        
        foreach (var item in Data)
        {
            sb.Append($"{item},");
        }
        
        // Remove the trailing comma.
        sb.Length -= 1;
        
        return sb.ToString().GetHashCode();
    }
    
    /// <inheritdoc cref="IParseableVersion{TSelf}.Parse"/>
    /// <remarks><paramref name="input"/> may begin with a 'v' (case-insensitive) for styling. The 'v' will be ignored.</remarks>
    public static SimpleVersion Parse([NotNull] string? input)
    {
        if (input is null || input.Length < 1)
        {
            throw new ArgumentNullException(nameof(input));
        }
        
        if (input.ToLower()[0] is 'v') input = input[1..];
        string[] inputSplit = input.Split('.');
        int len = inputSplit.Length;
        
        uint[] data = new uint[len];
        
        for (int i = 0; i < len; i++)
        {
            data[i] = UInt32.Parse(inputSplit[i]);
        }
        
        return new SimpleVersion(data);
    }
    
    /// <inheritdoc cref="IParseableVersion{TSelf}.TryParse"/>
    /// <remarks><paramref name="input"/> may begin with a 'v' (case-insensitive) for styling. The 'v' will be ignored.</remarks>
    public static bool TryParse([NotNullWhen(true)] string? input, out SimpleVersion? result)
    {
        // Call this default implementation, which will call SimpleVersion.Parse().
        return IParseableVersion<SimpleVersion>.TryParse(input, out result);
    }
}

/// <summary>
/// An <see cref="Exception"/> that is thrown when a versioning related error occurs.
/// </summary>
public class VersioningException : Exception
{
    public VersioningException() { }
    public VersioningException(string Message) : base(Message) { }
    public VersioningException(string Message, Exception inner) : base(Message, inner) { }
}
