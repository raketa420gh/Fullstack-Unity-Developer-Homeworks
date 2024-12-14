using NUnit.Framework;

public class ResourceConverterTests
{
    [Test]
    public void Instantiate()
    {
        //Arrange:
        var resourceIn = ResourceType.Log;
        var resourceOut = ResourceType.Plank;
        var resourceConverter = new ResourceConverter(resourceIn, resourceOut);

        //Assert:
        Assert.IsNotNull(resourceConverter);
    }

    [Test]
    public void WhenLaunchWithoutResourceInThenFalse()
    {
        //Arrange:
        var resourceIn = ResourceType.Log;
        var resourceOut = ResourceType.Plank;
        var resourceConverter = new ResourceConverter(resourceIn, resourceOut);
        
        //Act:
        resourceConverter.Launch();
        
        //Assert:
        Assert.IsFalse(resourceConverter.IsLaunched);
    }
}