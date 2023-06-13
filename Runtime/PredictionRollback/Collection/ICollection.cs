namespace UPR.PredictionRollback
{
    public interface ICollection<TEntry> : IReadOnlyCollection<TEntry>
    {
        public void Add(TEntry entry);
        public void Remove(TEntry entry);
    }
}
