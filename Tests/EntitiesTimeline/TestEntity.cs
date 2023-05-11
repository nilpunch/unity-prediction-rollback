﻿namespace UPR.Tests
{
    public class TestEntity : Entity
    {
        public TestEntity(EntityId id, int value) : base(id)
        {
            SimpleObject = new SimpleObject(value);

            LocalHistories.AddHistory(new ReversibleMemoryHistory<SimpleMemory>(SimpleObject));
        }

        public SimpleObject SimpleObject { get; }
    }
}