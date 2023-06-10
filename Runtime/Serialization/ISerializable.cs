namespace UPR.Serialization
{
    public interface ISerializable
    {
        void Serialize(IWriteHandle writeHandle);
    }
}
