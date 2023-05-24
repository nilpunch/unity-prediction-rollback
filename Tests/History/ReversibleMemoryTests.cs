using System;
using NUnit.Framework;

namespace UPR.Tests
{
    public class ReversibleMemoryTests
    {
        [Test]
        public void RollbackWithNotFilledHistoryShouldThrow()
        {
            // Arrange
            int originalValue = 11;
            var simpleObject = new TestObject(originalValue);
            var stateHistory = new MemoryHistory<TestObjectMemory>(simpleObject);

            // Act
            simpleObject.ChangeValue(0);
            TestDelegate performZeroRollback = () => stateHistory.Rollback(0);
            TestDelegate performRollback = () => stateHistory.Rollback(1);

            // Assert
            Assert.DoesNotThrow(performZeroRollback);
            Assert.Throws<Exception>(performRollback);
        }

        [Test]
        public void RollbackZeroAppliesLastSavedState()
        {
            // Arrange
            int originalValue = 11;
            var simpleObject = new TestObject(originalValue);
            var stateHistory = new MemoryHistory<TestObjectMemory>(simpleObject);

            // Act
            simpleObject.ChangeValue(3);
            stateHistory.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, simpleObject.Value);
        }

        [Test]
        public void RollbackWorksAsIntended()
        {
            // Arrange
            int originalValue = 11;
            var simpleObject = new TestObject(originalValue);
            var stateHistory = new MemoryHistory<TestObjectMemory>(simpleObject);

            // Act
            simpleObject.ChangeValue(0);
            stateHistory.SubmitStep();
            stateHistory.SubmitStep();
            stateHistory.Rollback(2);

            // Assert
            Assert.AreEqual(originalValue, simpleObject.Value);
        }
    }
}
