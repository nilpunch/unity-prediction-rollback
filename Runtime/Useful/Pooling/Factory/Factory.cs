namespace UPR.Useful
{
    public class Factory<T> : IFactory<T> where T : new()
    {
        public static IFactory<T> Default { get; } = new Factory<T>();

        public T Create()
        {
            return new T();
        }
    }
}
