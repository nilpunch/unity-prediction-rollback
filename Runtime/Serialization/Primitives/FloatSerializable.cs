namespace UPR.Serialization
{
    public class FloatSerializable : ISerializable
    {
        private readonly float _value;

        public FloatSerializable(float value)
        {
            _value = value;
        }

        public void Serialize(IWriteHandle writeHandle)
        {
            writeHandle.WriteFloat(_value);
        }
    }
}
