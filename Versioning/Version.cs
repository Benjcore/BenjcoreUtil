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

  public override string? ToString() {
    return InnerObject.ToString();
  }
  
}