namespace Tools
{
    public interface IPool<TItem> : IPoolReturn<TItem>
    {
        TItem Get();
        public void ReturnAll();
    }
}