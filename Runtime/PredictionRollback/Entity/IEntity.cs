namespace UPR.PredictionRollback
{
    public interface IEntity : IHistory, ISimulation, IRollback, IRebase
    {
        int LocalStep { get; }
    }
}
