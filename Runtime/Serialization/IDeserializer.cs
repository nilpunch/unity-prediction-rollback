namespace UPR.Serialization
{
    public interface IDeserializer<out TObject>
    {
        TObject Deserialize(IReadHandle readHandle);
    }
}
