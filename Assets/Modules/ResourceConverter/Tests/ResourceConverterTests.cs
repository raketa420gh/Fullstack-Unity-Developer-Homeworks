using NUnit.Framework;

namespace Modules.ResourceConverter
{
    public class ResourceConverterTests
    {
        [Test]
        public void Instantiate()
        {
            //Arrange:
            var resourceConverter = new ResourceConverter();
            
            //Assert:
            Assert.IsNotNull(resourceConverter);
        }
        
        public void InstantiateWithInvalidArgs(int inputCapacity, int outputCapacity)
        {
            
        }

        [Test]
        public void StartConvertingWithResource()
        {
            //Arrange:
            var resourceConverter = new ResourceConverter();

            //Act:
            resourceConverter.AddInputResource(1, out int changeCount);
            resourceConverter.StartConverting();

            //Assert:
            Assert.IsTrue(resourceConverter.IsConverting);
        }

        [Test]
        public void WhenAddToInputZoneResourcesCountMoreThanFreeSlotsThenReturnsItBack()
        {
            //Arrange:
            var inputCapacity = 3;
            var outputCapacity = 3;
            var resourceConverter = new ResourceConverter(inputCapacity, outputCapacity);
            var resourcesAddCount = 10;
            
            //Act:
            resourceConverter.AddInputResource(resourcesAddCount, out int changeCount);
            
            //Assert:
            Assert.Equals(changeCount, resourcesAddCount - resourceConverter.GetFreeInputResourceCount());
        }
    }
}