namespace UPR.Samples
{
    public interface IPool<TItem> : IPoolReturn<TItem>
    {
        TItem Get();
        public void ReturnAll();
    }
}