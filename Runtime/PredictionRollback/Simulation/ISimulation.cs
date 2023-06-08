namespace UPR.PredictionRollback
{
    /// <summary>
    /// Stepper for general simulated objects and for reversible simulation.
    /// </summary>
    public interface ISimulation
    {
        void StepForward();
    }
}
