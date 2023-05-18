namespace UPR.Samples
{
    public class IdGenerator : Entity, IIdGenerator, IMemory<IdGeneratorMemory>
    {
        private IdGeneratorMemory _memory;

        public IdGenerator(int startId) : base(new EntityId(startId))
        {
            _memory.IdCounter = startId + 1;
            LocalReversibleHistories.AddHistory(new ReversibleMemoryHistory<IdGeneratorMemory>(this));
        }

        public EntityId Generate()
        {
            int id = _memory.IdCounter;
            _memory.IdCounter += 1;
            return new EntityId(id);
        }

        public IdGeneratorMemory Save()
        {
            return _memory;
        }

        public void Load(IdGeneratorMemory memory)
        {
            _memory = memory;
        }
    }
}
