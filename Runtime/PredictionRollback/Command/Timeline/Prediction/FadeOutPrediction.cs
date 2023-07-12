using System;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class FadeOutPrediction<TCommand> : IReadOnlyCommandTimeline<TCommand> where TCommand : IFadingOutCommand<TCommand>
    {
        private readonly IReadOnlyCommandTimeline<TCommand> _commandTimeline;
        private readonly int _startDecayTick;
        private readonly int _decayDurationTicks;
        private readonly IEqualityComparer<TCommand> _equalityComparer;

        public FadeOutPrediction(IReadOnlyCommandTimeline<TCommand> commandTimeline, int startDecayTick = 30, int decayDurationTicks = 60, IEqualityComparer<TCommand> equalityComparer = null)
        {
            if (startDecayTick <= 0)
                throw new ArgumentOutOfRangeException(nameof(startDecayTick));

            if (decayDurationTicks <= 0)
                throw new ArgumentOutOfRangeException(nameof(decayDurationTicks));

            _commandTimeline = commandTimeline;
            _startDecayTick = startDecayTick;
            _decayDurationTicks = decayDurationTicks;
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

            int ticksPassed = tick - lastTickWithCommand;

            float fadeOutPercent = Math.Clamp(ticksPassed - _startDecayTick, 0, _decayDurationTicks) / (float)_decayDurationTicks;

            return _commandTimeline.GetCommand(lastTickWithCommand).FadeOut(fadeOutPercent);
        }
    }
}
