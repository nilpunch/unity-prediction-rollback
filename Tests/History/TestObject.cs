namespace UPR.Tests
{
    public class TestObject : IMemory<TestObjectMemory>
    {
        private TestObjectMemory _memory;

        public int Value => _memory.Value;

        public TestObject(int value)
        {
            _memory = new TestObjectMemory(value);
        }

        public void ChangeValue(int value)
        {
            _memory.Value = value;
        }

        public TestObjectMemory Save()
        {
            return _memory;
        }

        public void Load(TestObjectMemory memory)
        {
            _memory = memory;
        }
    }
}
