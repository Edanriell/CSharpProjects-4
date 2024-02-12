using EntityModels;

namespace UnitTests;

public class ControllerUnitTests
{
    [Fact]
    public void ControllerTest()
    {
        using NorthwindContext db = new();
        Assert.True(db.Database.CanConnect());
    }
}
