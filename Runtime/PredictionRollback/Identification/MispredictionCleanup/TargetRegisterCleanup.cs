namespace UPR.PredictionRollback
{
    public class TargetRegisterCleanup<TTarget> : IMispredictionCleanup where TTarget : ITickCounter
    {
        private readonly ICommandTargetRegistry<TTarget> _commandTargetRegistry;

        public TargetRegisterCleanup(ICommandTargetRegistry<TTarget> commandTargetRegistry)
        {
            _commandTargetRegistry = commandTargetRegistry;
        }

        public void CleanUp()
        {
            for (int i = _commandTargetRegistry.Entries.Count - 1; i >= 0; i--)
            {
                var entity = _commandTargetRegistry.Entries[i];
                if (entity.CurrentTick <= 0)
                {
                    _commandTargetRegistry.Remove(_commandTargetRegistry.GetTargetId(entity));
                }
            }
        }
    }
}
