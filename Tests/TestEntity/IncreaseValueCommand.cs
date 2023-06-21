namespace UPR.Tests
{
    public struct IncreaseValueCommand
    {
        public IncreaseValueCommand(int delta)
        {
            Delta = delta;
        }

        public int Delta { get; }
    }
}
