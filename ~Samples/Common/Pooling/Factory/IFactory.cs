namespace UPR.Samples
{
    public interface IFactory<out T>
    {
        T Create();
    }
}
