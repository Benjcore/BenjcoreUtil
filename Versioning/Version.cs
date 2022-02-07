using System.Runtime.CompilerServices;

namespace BenjcoreUtil.Versioning; 

public class Version : IVersionType<Version> {
  
  public IVersionType<object> InnerObject { get; }

  public Version(object VersionType) {

    if (VersionType is not IVersionType<object> type) {
      throw new VersioningException("Parameter does not implement IVersionType<T>." +
                                    $"\nGiven : {VersionType.GetType().ToString()}");
    }

    InnerObject = type;

  }

  public bool isNewerThan(Version input) {
    return InnerObject.isNewerThan(input.InnerObject);
  }

  public bool isNewerThanOrEqualTo(Version input) {
    return InnerObject.isNewerThanOrEqualTo(input.InnerObject);
  }

  public bool isOlderThan(Version input) {
    return InnerObject.isOlderThan(input.InnerObject);
  }

  public bool isOlderThanOrEqualTo(Version input) {
    return InnerObject.isOlderThanOrEqualTo(input.InnerObject);
  }

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
    
    return RuntimeHelpers.Equals(this, obj);
    
  }

  public override int GetHashCode() {
    return InnerObject.GetHashCode();
  }
  
}