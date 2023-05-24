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
            entitiesTimeline.SubmitStep();
            testEntity.TestObject.ChangeValue(22);
            entitiesTimeline.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, testEntity.TestObject.Value);
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
            entitiesTimeline.SubmitStep();

            int newValue = 22;
            testEntity.TestObject.ChangeValue(newValue);
            entitiesTimeline.SubmitStep();

            testEntity.TestObject.ChangeValue(33);
            entitiesTimeline.SubmitStep();

            entitiesTimeline.Rollback(1);

            // Assert
            Assert.AreEqual(newValue, testEntity.TestObject.Value);
        }
    }
}
