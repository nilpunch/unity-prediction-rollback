namespace UPR.Tests
{
    public class SimpleObject : IMemory<SimpleMemory>
    {
        private SimpleMemory _memory;

        public int Value => _memory.Value;

        public SimpleObject(int value)
        {
            _memory = new SimpleMemory(value);
        }

        public void ChangeValue(int value)
        {
            _memory.Value = value;
        }

        public SimpleMemory Save()
        {
            return _memory;
        }

        public void Load(in SimpleMemory memory)
        {
            _memory = memory;
        }
    }
}
