using UPR.Serialization;

namespace UPR.Networking
{
    public interface ICommandMarshal
    {
        void DeserializeAndForward(ReadHandle commandData);
    }
}