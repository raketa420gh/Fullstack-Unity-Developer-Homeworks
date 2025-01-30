using System;
using NUnit.Framework;

namespace Modules.ResourceConverter
{
    public class ResourceConverterTests
    {
        [TestCase(1, 100)]
        [TestCase(100, 1)]
        [TestCase(3, 73)]
        public void Instantiate(int inputCapacity, int outputCapacity)
        {
            //Arrange:
            var resourceConverter = new ResourceConverter(ResourceType.Logs, ResourceType.Planks, 
                inputCapacity, outputCapacity);
            
            //Assert:
            Assert.IsNotNull(resourceConverter);
            Assert.AreEqual(inputCapacity, resourceConverter.InputCapacity);
            Assert.AreEqual(outputCapacity, resourceConverter.OutputCapacity);
            Assert.AreEqual(0, resourceConverter.InputCount);
            Assert.AreEqual(0, resourceConverter.OutputCount);
        }
        
        [TestCase(-1, 1)]
        [TestCase(3, -5)]
        [TestCase(-7, -9)]
        [TestCase(0, 10)]
        [TestCase(15, 0)]
        public void WhenInstantiateWithInvalidCapacityThenException(int inputCapacity, int outputCapacity)
        {
            //Assert:
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                var _ = new ResourceConverter(ResourceType.Logs, ResourceType.Planks, 
                    inputCapacity, outputCapacity);
            });
        }

        [TestCase(0)]
        [TestCase(-50)]
        public void WhenInstantiateWithInvalidConvertingDurationThenException(float convertingDuration)
        {
            //Assert:
            Assert.Catch<ArgumentOutOfRangeException>(() =>
            {
                var _ = new ResourceConverter(ResourceType.Logs, ResourceType.Planks, 
                    1, 1, convertingDuration);
            });
        }
        
        [Test]
        public void WhenInstantiateWithInputOutputSameTypeThenException()
        {
            //Arrange:
            var inputType = ResourceType.Logs;
            var outputType = inputType;
            
            //Assert:
            Assert.Catch<ArgumentException>(() =>
            {
                var _ = new ResourceConverter(inputType, outputType);
            });
        }
    }
}