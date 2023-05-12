namespace Tools
{
    public interface IPoolFactory<out T>
    {
        T Create();
    }
}