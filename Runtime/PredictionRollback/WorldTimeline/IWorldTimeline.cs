namespace UPR.PredictionRollback
{
    public interface IWorldTimeline
    {
        void UpdateEarliestApprovedTick(int tick);
        void FastForwardToTick(int targetTick);
    }
}
