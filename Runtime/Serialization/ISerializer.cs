namespace UPR.Serialization
{
    public interface ISerializer<in TObject>
    {
        void Serialize(WriteHandle writeHandle, TObject value);
    }
}
