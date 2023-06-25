using UPR.Networking;
using UPR.PredictionRollback;
using UPR.Useful;

namespace UPR.Tests
{
    public class TestEntity : Entity, ICommandPlayer,
        ICommandTarget<IncreaseValueCommand>
    {
        private readonly FrequentlyChangingValue<int> _storedValue;

        private readonly ICommandTimeline<IncreaseValueCommand> _commandTimeline;

        public ICommandTimeline<IncreaseValueCommand> CommandTimeline => _commandTimeline;

        public TestEntity(int value)
        {
            _storedValue = new FrequentlyChangingValue<int>(value);

            _commandTimeline = new CommandTimeline<IncreaseValueCommand>();

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

        public void PlayCommands(int tick)
        {
            if (_commandTimeline.HasCommand(tick))
            {
                var increaseValueCommand = _commandTimeline.GetCommand(tick);

                StoredValue += increaseValueCommand.Delta;
            }
        }
    }
}
