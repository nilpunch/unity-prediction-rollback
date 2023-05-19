using System;
using NUnit.Framework;

namespace UPR.Tests
{
    public class EntityWorldTests
    {
        [Test]
        public void RollbackZeroAppliesLastSavedState()
        {
            // Arrange
            var entitiesTimeline = new EntityWorld<SimpleTestEntity>();
            int originalValue = 11;
            var testEntity = new SimpleTestEntity(originalValue);

            // Act
            entitiesTimeline.RegisterEntity(testEntity, new EntityId(0));
            entitiesTimeline.SaveStep();
            testEntity.SimpleObject.ChangeValue(22);
            entitiesTimeline.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, testEntity.SimpleObject.Value);
        }

        [Test]
        public void RollbackAliveEntityRollbackIt()
        {
            // Arrange
            var entitiesTimeline = new EntityWorld<SimpleTestEntity>();
            int originalValue = 11;
            var testEntity = new SimpleTestEntity(originalValue);

            // Act
            entitiesTimeline.RegisterEntity(testEntity, new EntityId(0));
            entitiesTimeline.SaveStep();

            int newValue = 22;
            testEntity.SimpleObject.ChangeValue(newValue);
            entitiesTimeline.SaveStep();

            testEntity.SimpleObject.ChangeValue(33);
            entitiesTimeline.SaveStep();

            entitiesTimeline.Rollback(1);

            // Assert
            Assert.AreEqual(newValue, testEntity.SimpleObject.Value);
        }
    }
}
