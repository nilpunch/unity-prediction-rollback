namespace UPR.Tests
{
    public class SimpleTestEntity : Entity
    {
        public SimpleTestEntity(int value) : base(0)
        {
            TestObject = new TestObject(value);

            var simpleObjectReversibleHistory = new MemoryHistory<TestObjectMemory>(TestObject);

            LocalReversibleHistories.AddHistory(simpleObjectReversibleHistory);
        }

        public TestObject TestObject { get; }
    }
}
