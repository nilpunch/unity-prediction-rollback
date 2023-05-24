using NUnit.Framework;

namespace UPR.Tests
{
    public class SimpleObjectTests
    {
        [Test]
        public void SimpleObjectChangesValue()
        {
            // Arrange
            int originalValue = 11;
            var simpleObject = new TestObject(originalValue);

            // Act
            simpleObject.ChangeValue(0);

            // Assert
            Assert.AreNotEqual(originalValue, simpleObject.Value);
        }
    }
}