namespace UPR.Samples
{
    public class FrequentlyChangedComponent<TData> : SnapshotComponent<TData>
    {
        protected override IValueHistory<TData> CreateHistory => new ValueHistory<TData>(InitialData);

        protected virtual TData InitialData { get; } = default;
    }
}
