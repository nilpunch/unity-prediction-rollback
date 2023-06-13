namespace UPR.PredictionRollback
{
    public class PredictionCommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly ICommandTimeline<TCommand> _commandTimeline;

        public PredictionCommandTimeline(ICommandTimeline<TCommand> commandTimeline)
        {
            _commandTimeline = commandTimeline;
        }

        public int GetLatestTickWithCommandBefore(int tickInclusive)
        {
            if (_commandTimeline.GetLatestTickWithCommandBefore(tickInclusive) == -1)
                return -1;

            return tickInclusive;
        }

        public bool HasCommand(int tick)
        {
            return _commandTimeline.GetLatestTickWithCommandBefore(tick) != -1;
        }

        public TCommand GetCommand(int tick)
        {
            int lastTickWithCommand = _commandTimeline.GetLatestTickWithCommandBefore(tick);
            return _commandTimeline.GetCommand(lastTickWithCommand);
        }

        public void RemoveAllCommandsDownTo(int tick)
        {
            _commandTimeline.RemoveAllCommandsDownTo(tick);
        }

        public void RemoveCommand(int tick)
        {
            _commandTimeline.RemoveCommand(tick);
        }

        public void InsertCommand(int tick, in TCommand command)
        {
            _commandTimeline.InsertCommand(tick, command);
        }
    }
}
