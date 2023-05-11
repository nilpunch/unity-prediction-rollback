namespace UPR.Samples
{
    public class Time : ISimulation
    {
        private long _tick;

        public void StepForward()
        {
            _tick += 1;
        }
    }
}
