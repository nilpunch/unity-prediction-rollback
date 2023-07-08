namespace UPR.Useful
{
    public interface IPool<TItem> : IPoolReturn<TItem>
    {
        TItem Get();
    }
}
