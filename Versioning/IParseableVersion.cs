namespace BenjcoreUtil.Versioning;

public interface IParseableVersion<out TSelf> where TSelf : IParseableVersion<TSelf>
{
    /// <summary>
    /// Converts the string representation of a version to its object equivalent.
    /// </summary>
    /// <param name="input">A string representation of the version.</param>
    /// <returns>An instance of this class that is the equivalent of <paramref name="input"/>.</returns>
    /// <exception cref="ArgumentNullException">input is null or empty.</exception>
    /// <exception cref="FormatException">input is not in the correct format.</exception>
    /// <exception cref="OverflowException">
    /// A number in input is not within the 32-Bit signed integer limit.
    /// </exception>
    public static abstract TSelf Parse(string? input);
    
    /// <summary>
    /// Attempts to convert a string representation of a version to its object equivalent.
    /// </summary>
    /// <param name="input">A string representation of the version.</param>
    /// <param name="result">
    /// When this method returns, contains the object equivalent of the string
    /// <paramref name="input"/> if the conversion succeeded, or null if the conversion failed.
    /// </param>
    /// <returns>True if <paramref name="input"/> was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string? input, out TSelf? result)
    {
        try
        {
            result = TSelf.Parse(input);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }
}