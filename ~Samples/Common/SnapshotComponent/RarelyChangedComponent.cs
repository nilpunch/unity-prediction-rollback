namespace UPR.Samples
{
    public class RarelyChangedComponent<TData> : SnapshotComponent<TData>
    {
        protected override IValueHistory<TData> CreateHistory => new ValueChangeOnlyHistory<TData>(InitialData);

        protected virtual TData InitialData { get; } = default;
    }
}
