namespace UPR.PredictionRollback
{
    public class PredictionCommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly ICommandTimeline<TCommand> _commandTimeline;

        public PredictionCommandTimeline(ICommandTimeline<TCommand> commandTimeline)
        {
            _commandTimeline = commandTimeline;
        }

        public int GetLatestTickWithCommandInclusiveBefore(int tick)
        {
            if (_commandTimeline.GetLatestTickWithCommandInclusiveBefore(tick) == -1)
                return -1;

            return tick;
        }

        public bool HasCommand(int tick)
        {
            return _commandTimeline.GetLatestTickWithCommandInclusiveBefore(tick) != -1;
        }

        public TCommand GetCommand(int tick)
        {
            int lastTickWithCommand = _commandTimeline.GetLatestTickWithCommandInclusiveBefore(tick);
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
