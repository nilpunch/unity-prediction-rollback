namespace UPR.PredictionRollback
{
    public interface IFadingOutCommand<TCommand>
    {
        /// <summary>
        /// Transform command magnitude to achieve smooth fading out.
        /// </summary>
        /// <param name="percent">
        /// 0 - no decay, full magnitude; 1 - full decay, zero magnitude.
        /// </param>
        /// <returns></returns>
        TCommand FadeOut(float percent);
    }
}
