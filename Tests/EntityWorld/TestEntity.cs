namespace UPR.Tests
{
    public class TestEntity : Entity
    {
        private readonly FrequentlyChangingValue<int> _storedValue;

        public TestEntity(int value)
        {
            _storedValue = new FrequentlyChangingValue<int>(value);

            LocalHistories.Add(_storedValue);
            LocalRollbacks.Add(_storedValue);
            LocalRebases.Add(_storedValue);
        }

        public int StoredValue
        {
            get
            {
                return _storedValue.Value;
            }
            set
            {
                _storedValue.Value = value;
            }
        }
    }
}
