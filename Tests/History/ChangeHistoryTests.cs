using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UPR.Tests
{
    public class ChangeHistoryTests
    {
        [Test]
        public void RollbackWithNotFilledHistoryShouldThrow()
        {
            // Arrange
            int originalValue = 11;
            var stateHistory = new ChangeHistory<int>(originalValue);

            // Act
            stateHistory.Value = 0;
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
            var stateHistory = new ChangeHistory<int>(originalValue);

            // Act
            stateHistory.Value = 0;
            stateHistory.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, stateHistory.Value);
        }

        [Test]
        public void RollbackWorksAsIntended()
        {
            // Arrange
            int originalValue = 11;
            var stateHistory = new ChangeHistory<int>(originalValue);

            // Act
            stateHistory.Value = 0;
            stateHistory.SubmitStep();
            stateHistory.SubmitStep();
            stateHistory.Rollback(2);

            // Assert
            Assert.AreEqual(originalValue, stateHistory.Value);
        }
    }
}
