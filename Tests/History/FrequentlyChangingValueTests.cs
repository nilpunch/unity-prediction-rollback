using System;
using NUnit.Framework;
using UPR.Useful;

namespace UPR.Tests
{
    public class FrequentlyChangingValueTests
    {
        [Test]
        public void Rollback_WithNotFilledHistory_ShouldThrow()
        {
            // Arrange
            int originalValue = 11;
            var stateHistory = new FrequentlyChangingValue<int>(originalValue);

            // Act
            stateHistory.Value = 0;
            TestDelegate performZeroRollback = () => stateHistory.Rollback(0);
            TestDelegate performRollback = () => stateHistory.Rollback(1);

            // Assert
            Assert.DoesNotThrow(performZeroRollback);
            Assert.Throws<Exception>(performRollback);
        }

        [Test]
        public void RollbackZero_AppliesLastSavedState()
        {
            // Arrange
            int originalValue = 11;
            var stateHistory = new FrequentlyChangingValue<int>(originalValue);

            // Act
            stateHistory.Value = 0;
            stateHistory.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, stateHistory.Value);
        }

        [Test]
        public void Rollback_WithFilledHistory_AppliesPreviousState()
        {
            // Arrange
            int originalValue = 11;
            var stateHistory = new FrequentlyChangingValue<int>(originalValue);

            // Act
            stateHistory.Value = 0;
            stateHistory.SaveStep();
            stateHistory.SaveStep();
            stateHistory.Rollback(2);

            // Assert
            Assert.AreEqual(originalValue, stateHistory.Value);
        }
    }
}
