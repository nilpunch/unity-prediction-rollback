using System;
using NUnit.Framework;

namespace UPR.Tests
{
    public class EntitiesTimelineTests
    {
        [Test]
        public void RollbackZeroAppliesLastSavedState()
        {
            // Arrange
            var entitiesTimeline = new EntitiesTimeline();
            int originalValue = 11;
            var testEntity = new TestEntity(new EntityId(0), originalValue);

            entitiesTimeline.RegisterEntity(testEntity);

            // Act
            testEntity.SimpleObject.ChangeValue(22);
            entitiesTimeline.Rollback(0);

            Assert.AreEqual(originalValue, testEntity.SimpleObject.Value);
        }

        [Test]
        public void RollbackWorksAsIntended()
        {
            // Arrange
            var entitiesTimeline = new EntitiesTimeline();
            int originalValue = 11;
            var testEntity = new TestEntity(new EntityId(0), originalValue);

            entitiesTimeline.RegisterEntity(testEntity);

            // Act
            int newValue = 22;
            testEntity.SimpleObject.ChangeValue(newValue);
            entitiesTimeline.StepForward(0);
            entitiesTimeline.SaveState();

            testEntity.SimpleObject.ChangeValue(33);
            entitiesTimeline.StepForward(1);
            entitiesTimeline.SaveState();

            entitiesTimeline.Rollback(1);

            Assert.AreEqual(newValue, testEntity.SimpleObject.Value);
        }
    }
}
