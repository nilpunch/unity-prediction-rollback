namespace UPR.Samples
{
    public class IdGenerator : Entity, IIdGenerator, IMemory<IdGeneratorMemory>
    {
        private IdGeneratorMemory _memory;

        public IdGenerator(int startId)
        {
            _memory.IdCounter = startId;
            LocalReversibleHistories.AddReversibleHistory(new MemoryHistory<IdGeneratorMemory>(this));
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
