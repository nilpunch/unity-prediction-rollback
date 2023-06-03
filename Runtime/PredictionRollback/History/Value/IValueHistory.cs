namespace UPR
{
    public interface IValueHistory<TValue> : IHistory, IRollback
    {
        TValue Value { get; set; }
    }
}
