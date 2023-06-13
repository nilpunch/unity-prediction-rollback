using NUnit.Framework;

namespace UPR.Tests
{
    public class EntityTests
    {
        [Test]
        public void RollbackZero_AppliesLastSavedState()
        {
            // Arrange
            int originalValue = 11;
            var testEntity = new TestEntity(originalValue);

            // Act
            testEntity.SaveStep();
            testEntity.StoredValue = 22;
            testEntity.Rollback(0);

            // Assert
            Assert.AreEqual(originalValue, testEntity.StoredValue);
        }

        [Test]
        public void Rollback_EntityWithSavedStep_AppliesPreviousState()
        {
            // Arrange
            int originalValue = 11;
            var testEntity = new TestEntity(originalValue);

            // Act
            testEntity.SaveStep();

            int newValue = 22;
            testEntity.StoredValue = newValue;
            testEntity.SaveStep();

            testEntity.StoredValue = 33;
            testEntity.SaveStep();

            testEntity.Rollback(1);

            // Assert
            Assert.AreEqual(newValue, testEntity.StoredValue);
        }
    }
}
