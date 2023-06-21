using UPR.PredictionRollback;

namespace UPR.Networking
{
    public interface ICommandTarget<TCommand>
    {
        ICommandTimeline<TCommand> CommandTimeline { get; }
    }
}
