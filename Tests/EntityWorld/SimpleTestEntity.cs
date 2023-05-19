namespace UPR.Tests
{
    public class SimpleTestEntity : Entity
    {
        public SimpleTestEntity(int value)
        {
            SimpleObject = new SimpleObject(value);

            var simpleObjectReversibleHistory = new ReversibleMemoryHistory<SimpleMemory>(SimpleObject);

            LocalReversibleHistories.AddHistory(simpleObjectReversibleHistory);
        }

        public SimpleObject SimpleObject { get; }
    }
}
