namespace UPR
{
    public interface IMemory<TMemory>
    {
        TMemory Save();
        void Load(in TMemory memory);
    }
}
