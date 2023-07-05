using System;

namespace UPR.PredictionRollback
{
    public class RepeatPrediction<TCommand> : IReadOnlyCommandTimeline<TCommand> where TCommand : IEquatable<TCommand>
    {
        private readonly IReadOnlyCommandTimeline<TCommand> _commandTimeline;

        public RepeatPrediction(IReadOnlyCommandTimeline<TCommand> commandTimeline)
        {
            _commandTimeline = commandTimeline;
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
            return HasCommand(tick) && GetCommand(tick).Equals(command);
        }

        public TCommand GetCommand(int tick)
        {
            int lastTickWithCommand = _commandTimeline.GetLatestTickWithCommandBefore(tick);
            return _commandTimeline.GetCommand(lastTickWithCommand);
        }
    }
}
