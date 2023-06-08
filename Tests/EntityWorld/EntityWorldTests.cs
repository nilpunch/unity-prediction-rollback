﻿using System;
using NUnit.Framework;
using UPR.PredictionRollback;

namespace UPR.Tests
{
    public class EntityWorldTests
    {
        [Test]
        public void RollbackZero_AppliesLastSavedState()
        {
            // Arrange
            var entitiesTimeline = new EntityWorld<TestEntity>();
            int originalValue = 11;
            var testEntity = new TestEntity(originalValue);

            // Act
            entitiesTimeline.RegisterEntity(testEntity, new EntityId(0));
            entitiesTimeline.SaveStep();
            testEntity.StoredValue = 22;
            entitiesTimeline.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, testEntity.StoredValue);
        }

        [Test]
        public void Rollback_WithSavedStep_AppliesPreviousState()
        {
            // Arrange
            var entitiesTimeline = new EntityWorld<TestEntity>();
            int originalValue = 11;
            var testEntity = new TestEntity(originalValue);

            // Act
            entitiesTimeline.RegisterEntity(testEntity, new EntityId(0));
            entitiesTimeline.SaveStep();

            int newValue = 22;
            testEntity.StoredValue = newValue;
            entitiesTimeline.SaveStep();

            testEntity.StoredValue = 33;
            entitiesTimeline.SaveStep();

            entitiesTimeline.Rollback(1);

            // Assert
            Assert.AreEqual(newValue, testEntity.StoredValue);
        }
    }
}
