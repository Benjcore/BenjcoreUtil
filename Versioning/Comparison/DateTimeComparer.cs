namespace BenjcoreUtil.Versioning.Comparison;

/// <summary>
/// A build <see cref="DateTime"/> based version comparer where the later build date time is considered newer.
/// </summary>
/// <param name="build_date_time">The current build <see cref="DateTime"/>.</param>
public sealed class DateTimeComparer(DateTime build_date_time) : ComparableVersionBase<DateTimeComparer>
{
    /// <summary>
    /// The build <see cref="DateTime"/> of the current instance.
    /// </summary>
    public DateTime BuildDateTime => build_date_time;
    
    public override (bool NewerThan, bool EqualTo) Compare(DateTimeComparer other)
    {
        return (BuildDateTime > other.BuildDateTime, BuildDateTime == other.BuildDateTime);
    }
}