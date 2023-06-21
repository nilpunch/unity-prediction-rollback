using UPR.Networking;
using UPR.PredictionRollback;
using UPR.Useful;

namespace UPR.Tests
{
    public class TestEntity : Entity, ICommandPlayer,
        ICommandTarget<IncreaseValueCommand>
    {
        private readonly FrequentlyChangingValue<int> _storedValue;

        public ICommandTimeline<IncreaseValueCommand> CommandTimeline { get; }

        public TestEntity(int value)
        {
            _storedValue = new FrequentlyChangingValue<int>(value);

            CommandTimeline = new CommandTimeline<IncreaseValueCommand>();

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
            if (CommandTimeline.HasCommand(tick))
            {
                var increaseValueCommand = CommandTimeline.GetCommand(tick);

                StoredValue += increaseValueCommand.Delta;
            }
        }
    }
}
