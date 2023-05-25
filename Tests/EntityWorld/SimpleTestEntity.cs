namespace UPR.Tests
{
    public class SimpleTestEntity : Entity
    {
        public SimpleTestEntity(int value)
        {
            TestObject = new TestObject(value);

            var simpleObjectReversibleHistory = new MemoryHistory<TestObjectMemory>(TestObject);

            LocalReversibleHistories.AddHistory(simpleObjectReversibleHistory);
        }

        public TestObject TestObject { get; }
    }
}
