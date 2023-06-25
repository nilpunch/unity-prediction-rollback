﻿using System;

namespace UPR.PredictionRollback
{
    public class FadeOutPrediction<TCommand> : CommandTimelineDecorator<TCommand> where TCommand : IEquatable<TCommand>, IDecayingCommand<TCommand>
    {
        private readonly int _startDecayTick;
        private readonly int _decayDurationTicks;

        public FadeOutPrediction(ICommandTimeline<TCommand> commandTimeline, int startDecayTick = 30, int decayDurationTicks = 60) : base(commandTimeline)
        {
            if (startDecayTick <= 0)
                throw new ArgumentOutOfRangeException(nameof(startDecayTick));

            if (decayDurationTicks <= 0)
                throw new ArgumentOutOfRangeException(nameof(decayDurationTicks));

            _startDecayTick = startDecayTick;
            _decayDurationTicks = decayDurationTicks;
        }

        public override bool HasCommand(int tick)
        {
            return CommandTimeline.GetLatestTickWithSolidCommandBefore(tick) != -1;
        }

        public override bool HasExactCommand(int tick, TCommand command)
        {
            return HasCommand(tick) && GetCommand(tick).Equals(command);
        }

        public override TCommand GetCommand(int tick)
        {
            int lastTickWithCommand = CommandTimeline.GetLatestTickWithSolidCommandBefore(tick);

            int ticksPassed = tick - lastTickWithCommand;

            float fadeOutPrecent = Math.Clamp(ticksPassed - _startDecayTick, 0, _decayDurationTicks) / (float)_decayDurationTicks;

            return CommandTimeline.GetCommand(lastTickWithCommand).FadeOutPercent(fadeOutPrecent);
        }
    }
}