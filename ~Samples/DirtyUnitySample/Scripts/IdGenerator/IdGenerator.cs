namespace UPR.Samples
{
    public class IdGenerator : Entity, IIdGenerator, IMemory<IdGeneratorMemory>
    {
        private IdGeneratorMemory _memory;

        public IdGenerator(int startId) : base(new EntityId(-1))
        {
            _memory.IdCounter = startId;
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

        public void Load(in IdGeneratorMemory memory)
        {
            _memory = memory;
        }
    }
}
