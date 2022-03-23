namespace BenjcoreUtil.Versioning;

/// <summary>
/// A wrapper class for classes that implement the <see cref="IVersionType{Self}"/> in most cases using this class
/// rather than the class of specific version type (eg <see cref="Simple"/>) will improve code readability.
/// </summary>
/// <remarks>
/// The inner object wrapped by this class can be referenced using the <see cref="InnerObject"/> property.
/// </remarks>
public class Version : IVersionType<Version> {
  
  /// <summary>
  /// The inner <see cref="IVersionType{Self}"/> object wrapped by the current <see cref="Version"/> object.
  /// </summary>
  public IVersionType<object> InnerObject { get; }

  /// <summary>
  /// Creates a new instance of the <see cref="Version"/> class.
  /// </summary>
  /// <param name="VersionType">An object that implements <see cref="IVersionType{Self}"/>.
  /// This will dictate the type and value of the <see cref="Version"/>.</param>
  /// <exception cref="VersioningException">
  /// <paramref name="VersionType"/> does not implement <see cref="IVersionType{Self}"/>.
  /// </exception>
  public Version(object VersionType) {

    // Ensure VersionType implements IVersionType
    if (VersionType is not IVersionType<object> type) {
      throw new VersioningException("Parameter does not implement IVersionType<T>." + 
                                    $"\nGiven : {VersionType.GetType().ToString()}");
    }

    InnerObject = type;

  }

  /// <summary>
  /// Checks whether the current object is newer than <paramref name="input"/>.
  /// </summary>
  /// <param name="input">The object you wish to compare to the current object.</param>
  /// <returns>
  /// True if the current object is newer than <paramref name="input"/>, otherwise false.
  /// </returns>
  /// <exception cref="VersioningException">Can be thrown by <see cref="InnerObject"/>.</exception>
  public bool isNewerThan(Version input) {
    return InnerObject.isNewerThan(input.InnerObject);
  }

  /// <summary>
  /// Checks whether the current object is newer than or equal to <paramref name="input"/>.
  /// </summary>
  /// <param name="input">The object you wish to compare to the current object.</param>
  /// <returns>
  /// True if the current object is newer than or equal to <paramref name="input"/>, otherwise false.
  /// </returns>
  /// <exception cref="VersioningException">Can be thrown by <see cref="InnerObject"/>.</exception>
  public bool isNewerThanOrEqualTo(Version input) {
    return InnerObject.isNewerThanOrEqualTo(input.InnerObject);
  }

  /// <summary>
  /// Checks whether the current object is older than <paramref name="input"/>.
  /// </summary>
  /// <param name="input">The object you wish to compare to the current object.</param>
  /// <returns>
  /// True if the current object is older than <paramref name="input"/>, otherwise false.
  /// </returns>
  /// <exception cref="VersioningException">Can be thrown by <see cref="InnerObject"/>.</exception>
  public bool isOlderThan(Version input) {
    return InnerObject.isOlderThan(input.InnerObject);
  }

  /// <summary>
  /// Checks whether the current object is older than or equal to <paramref name="input"/>.
  /// </summary>
  /// <param name="input">The object you wish to compare to the current object.</param>
  /// <returns>
  /// True if the current object is older than or equal to <paramref name="input"/>, otherwise false.
  /// </returns>
  /// <exception cref="VersioningException">Can be thrown by <see cref="InnerObject"/>.</exception>
  public bool isOlderThanOrEqualTo(Version input) {
    return InnerObject.isOlderThanOrEqualTo(input.InnerObject);
  }

  /// <summary>
  /// Checks whether the current object is equal to <paramref name="input"/>.
  /// </summary>
  /// <param name="input">The object you wish to compare to the current object.</param>
  /// <returns>
  /// True if the current object is is equal to <paramref name="input"/>, otherwise false.
  /// </returns>
  /// <exception cref="VersioningException">Can be thrown by <see cref="InnerObject"/>.</exception>
  public bool IsEqualTo(Version input) {
    return InnerObject.IsEqualTo(input.InnerObject);
  }
  
  // Operators
  public static explicit operator string?(Version input) => input.ToString();
  public static bool operator ==(Version x, Version y) { return x.IsEqualTo(y); }
  public static bool operator !=(Version x, Version y) { return !x.IsEqualTo(y); }
  public static bool operator >(Version x, Version y) { return x.isNewerThan(y); }
  public static bool operator <(Version x, Version y) { return x.isOlderThan(y); }
  public static bool operator >=(Version x, Version y) { return x.isNewerThanOrEqualTo(y); }
  public static bool operator <=(Version x, Version y) { return x.isOlderThanOrEqualTo(y); }

  public override string? ToString() {
    return InnerObject.ToString();
  }
  
  public override bool Equals(object? obj) {
    
    if (obj is Version version) {
      return InnerObject.Equals(version.InnerObject);
    }
    
    return false;
    
  }

  public override int GetHashCode() {
    return InnerObject.GetHashCode();
  }
  
}

/// <summary>
/// An <see cref="Exception"/> that is thrown when a versioning related error occurs.
/// </summary>
public class VersioningException : Exception {
  
  public VersioningException() { }

  public VersioningException(string Message) : base(Message) { }

  public VersioningException(string Message, Exception inner) : base(Message, inner) { }
  
}