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
        public void SimpleObjectChangesValue()
        {
            // Arrange
            int originalValue = 11;
            var simpleObject = new SimpleObject(originalValue);

            // Act
            simpleObject.ChangeValue(0);

            // Assert
            Assert.AreNotEqual(originalValue, simpleObject.Value);
        }

        [Test]
        public void RollbackWithNotFilledHistoryShouldThrow()
        {
            // Arrange
            int originalValue = 11;
            var simpleObject = new SimpleObject(originalValue);
            var stateHistory = new ReversibleMemoryHistory<SimpleMemory>(simpleObject);

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
            var simpleObject = new SimpleObject(originalValue);
            var stateHistory = new ReversibleMemoryHistory<SimpleMemory>(simpleObject);

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
            var simpleObject = new SimpleObject(originalValue);
            var stateHistory = new ReversibleMemoryHistory<SimpleMemory>(simpleObject);

            // Act
            simpleObject.ChangeValue(0);
            stateHistory.SaveStep();
            stateHistory.SaveStep();
            stateHistory.Rollback(2);

            // Assert
            Assert.AreEqual(originalValue, simpleObject.Value);
        }
    }
}