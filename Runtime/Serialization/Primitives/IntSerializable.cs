namespace UPR.Serialization
{
    public class IntSerializable : ISerializable
    {
        private readonly int _value;

        public IntSerializable(int value)
        {
            _value = value;
        }

        public void Serialize(IWriteHandle writeHandle)
        {
            writeHandle.WriteInt(_value);
        }
    }
}
