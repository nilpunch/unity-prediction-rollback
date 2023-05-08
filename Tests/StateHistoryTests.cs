using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UPR.Tests
{
    public class StateHistoryTests
    {
        [Test]
        public void RollbackWithEmptyHistoryShouldRollbackToOriginal()
        {
            // Arrange
            int originalValue = 11;
            var simpleObject = new SimpleObject(originalValue);
            var stateHistory = new StateHistory<SimpleMemory>(simpleObject);

            // Act
            simpleObject.ChangeValue(0);
            stateHistory.Rollback(1);
            stateHistory.Rollback(2);

            // Assert
            Assert.Equals(simpleObject.Value, originalValue);
        }

        [Test]
        public void RollbackZeroAppliesLastSaveState()
        {
            // Arrange
            var simpleObject = new SimpleObject(11);
            var stateHistory = new StateHistory<SimpleMemory>(simpleObject);

            int lastSavedValue = 0;
            simpleObject.ChangeValue(lastSavedValue);
            stateHistory.SaveStep();

            // Act
            simpleObject.ChangeValue(3);
            stateHistory.Rollback(0);

            // Assert
            Assert.Equals(simpleObject.Value, lastSavedValue);
        }
    }
}
