using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

internal class WrongVersionType : IVersion
{
    public bool IsNewerThan(IVersion input)
    {
        throw new Exception(); // Should never be called.
    }
    
    public bool IsNewerThanOrEqualTo(IVersion input)
    {
        throw new Exception(); // Should never be called.
    }
    
    public bool IsOlderThan(IVersion input)
    {
        throw new Exception(); // Should never be called.
    }
    
    public bool IsOlderThanOrEqualTo(IVersion input)
    {
        throw new Exception(); // Should never be called.
    }
    
    public bool IsEqualTo(IVersion input)
    {
        throw new Exception(); // Should never be called.
    }
}

public class WrongTypeComparison
{
    /*
     * Because these test are very simple, we don't
     * need to follow the arrange/act/assert pattern.
     */
    
    private static readonly SimpleVersion SimpleVersion = new SimpleVersion([1]);
    private static readonly IVersion WrongVersionType = new WrongVersionType();
    
    [Fact]
    public void SimpleVersion_WrongTypeComparison_IsEqualTo_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => SimpleVersion.IsEqualTo(WrongVersionType));
    }
    
    [Fact]
    public void SimpleVersion_WrongTypeComparison_IsNewerThan_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => SimpleVersion.IsNewerThan(WrongVersionType));
    }
    
    [Fact]
    public void SimpleVersion_WrongTypeComparison_IsNewerThanOrEqualTo_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => SimpleVersion.IsNewerThanOrEqualTo(WrongVersionType));
    }
    
    [Fact]
    public void SimpleVersion_WrongTypeComparison_IsOlderThan_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => SimpleVersion.IsOlderThan(WrongVersionType));
    }
    
    [Fact]
    public void SimpleVersion_WrongTypeComparison_IsOlderThanOrEqualTo_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => SimpleVersion.IsOlderThanOrEqualTo(WrongVersionType));
    }
}