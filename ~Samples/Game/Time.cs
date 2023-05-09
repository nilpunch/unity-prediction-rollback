namespace UPR.Samples
{
    public class Time : ISimulation
    {
        private long _tick;
        
        public void StepForward(int currentTick)
        {
            _tick = currentTick;
        }
    }
}
