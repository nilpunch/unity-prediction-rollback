namespace UPR.Samples
{
    public struct SimulationSpeed
    {
        public int TicksPerSecond { get; }
        public float SecondsPerTick { get; }
        
        public SimulationSpeed(int ticksPerSecond)
        {
            TicksPerSecond = ticksPerSecond;
            SecondsPerTick = 1f / TicksPerSecond;
        }
    }
}
