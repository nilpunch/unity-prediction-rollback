namespace UPR
{
    public class PredictionCommandTimeline<TCommand> : ICommandTimeline<TCommand>
    {
        private readonly ICommandTimeline<TCommand> _commandTimeline;

        public PredictionCommandTimeline(ICommandTimeline<TCommand> commandTimeline)
        {
            _commandTimeline = commandTimeline;
        }

        public int GetLatestTickWithCommand(int tick)
        {
            return tick;
        }

        public void ExecuteCommand(int tick)
        {
            int lastTickWithCommand = _commandTimeline.GetLatestTickWithCommand(tick);
            _commandTimeline.ExecuteCommand(lastTickWithCommand);
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
