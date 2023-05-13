namespace Tools
{
    public interface IFactory<out T>
    {
        T Create();
    }
}
