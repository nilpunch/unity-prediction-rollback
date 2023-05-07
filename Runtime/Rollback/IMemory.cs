namespace UPR
{
    public interface IMemory<TSnapshot>
    {
        TSnapshot Save();
        void Load(in TSnapshot snapshot);
    }
}
