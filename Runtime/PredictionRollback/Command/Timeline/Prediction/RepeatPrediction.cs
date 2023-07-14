using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class RepeatPrediction<TCommand> : IReadOnlyCommandTimeline<TCommand>
    {
        private readonly IReadOnlyCommandTimeline<TCommand> _commandTimeline;
        private readonly IEqualityComparer<TCommand> _equalityComparer;

        public RepeatPrediction(IReadOnlyCommandTimeline<TCommand> commandTimeline, IEqualityComparer<TCommand> equalityComparer = null)
        {
            _commandTimeline = commandTimeline;
            _equalityComparer = equalityComparer ?? EqualityComparer<TCommand>.Default;
        }

        public int GetLatestTickWithCommandBefore(int tickInclusive)
        {
            if (_commandTimeline.GetLatestTickWithCommandBefore(tickInclusive) == -1)
            {
                return -1;
            }

            return tickInclusive;
        }

        public bool HasCommand(int tick)
        {
            return _commandTimeline.GetLatestTickWithCommandBefore(tick) != -1;
        }

        public bool HasExactCommand(int tick, TCommand command)
        {
            return HasCommand(tick) && _equalityComparer.Equals(GetCommand(tick), command);
        }

        public TCommand GetCommand(int tick)
        {
            int lastTickWithCommand = _commandTimeline.GetLatestTickWithCommandBefore(tick);
            return _commandTimeline.GetCommand(lastTickWithCommand);
        }
    }
}
