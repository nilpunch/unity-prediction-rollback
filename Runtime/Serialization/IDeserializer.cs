namespace UPR.Serialization
{
    public interface IDeserializer<out TObject>
    {
        TObject Deserialize(ReadHandle readHandle);
    }
}
