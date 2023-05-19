namespace UPR
{
    public interface IReversibleHistory : IHistory, IRollback
    {
        int StepsSaved { get; }
    }
}
