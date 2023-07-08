namespace UPR.Useful
{
    public interface IFactory<out T>
    {
        T Create();
    }
}
