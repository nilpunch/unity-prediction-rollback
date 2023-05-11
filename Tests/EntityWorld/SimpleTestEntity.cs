namespace UPR.Tests
{
    public class SimpleTestEntity : Entity
    {
        public SimpleTestEntity(EntityId id, int value) : base(id)
        {
            SimpleObject = new SimpleObject(value);

            var simpleObjectReversibleHistory = new ReversibleMemoryHistory<SimpleMemory>(SimpleObject);

            LocalReversibleHistories.AddHistory(simpleObjectReversibleHistory);
            LocalRollbacks.AddRollback(simpleObjectReversibleHistory);
        }

        public SimpleObject SimpleObject { get; }
    }
}
