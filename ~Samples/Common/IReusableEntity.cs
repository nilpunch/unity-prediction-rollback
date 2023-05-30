namespace UPR.Samples
{
    public interface IReusableEntity
    {
        bool CanBeReused { get; }
        void ResetLocalStep();
    }
}
