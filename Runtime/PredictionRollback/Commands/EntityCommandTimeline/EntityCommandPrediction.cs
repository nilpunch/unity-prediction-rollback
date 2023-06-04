namespace UPR
{
    public class EntityCommandPrediction<TCommand> : IEntityCommandTimeline<TCommand>
    {
        private readonly IEntityCommandTimeline<TCommand> _entityCommandTimeline;

        public EntityCommandPrediction(IEntityCommandTimeline<TCommand> entityCommandTimeline)
        {
            _entityCommandTimeline = entityCommandTimeline;
        }

        public int GetLatestTickWithCommand(int tick)
        {
            return tick;
        }

        public void ExecuteCommand(int tick)
        {
            int lastTickWithCommand = _entityCommandTimeline.GetLatestTickWithCommand(tick);
            _entityCommandTimeline.ExecuteCommand(lastTickWithCommand);
        }

        public void RemoveAllCommandsDownTo(int tick)
        {
            _entityCommandTimeline.RemoveAllCommandsDownTo(tick);
        }

        public void RemoveCommand(int tick)
        {
            _entityCommandTimeline.RemoveCommand(tick);
        }

        public void InsertCommand(int tick, in TCommand command)
        {
            _entityCommandTimeline.InsertCommand(tick, command);
        }
    }
}
