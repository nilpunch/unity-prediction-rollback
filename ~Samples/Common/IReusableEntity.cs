namespace UPR.Samples
{
    public interface IReusableEntity
    {
        bool CanBeReused { get; }

        /// <summary>
        /// Use only on mispredictions.
        /// </summary>
        void FullyResetEntity();
    }
}
