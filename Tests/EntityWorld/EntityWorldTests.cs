using System;
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
            var entityWorld = new CommandTargetRegistry<TestCommandTarget>();
            var worldHistory = new CollectionHistory(entityWorld);
            var worldRollback = new CollectionRollback(entityWorld);
            int originalValue = 11;
            var testEntity = new TestCommandTarget(originalValue);

            // Act
            entityWorld.Add(testEntity, new TargetId(0));
            worldHistory.SaveStep();
            testEntity.StoredValue = 22;
            worldRollback.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, testEntity.StoredValue);
        }

        [Test]
        public void Rollback_WithSavedStep_AppliesPreviousState()
        {
            // Arrange
            var entityWorld = new CommandTargetRegistry<TestCommandTarget>();
            var worldHistory = new CollectionHistory(entityWorld);
            var worldRollback = new CollectionRollback(entityWorld);
            int originalValue = 11;
            var testEntity = new TestCommandTarget(originalValue);

            // Act
            entityWorld.Add(testEntity, new TargetId(0));
            worldHistory.SaveStep();

            int newValue = 22;
            testEntity.StoredValue = newValue;
            worldHistory.SaveStep();

            testEntity.StoredValue = 33;
            worldHistory.SaveStep();

            worldRollback.Rollback(1);

            // Assert
            Assert.AreEqual(newValue, testEntity.StoredValue);
        }
    }
}
