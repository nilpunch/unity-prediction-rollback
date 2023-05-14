namespace UPR.Samples
{
    public static class EntityExtensions
    {
        public static void ResetHistory(this IEntity entity)
        {
            entity.Rollback(entity.CurrentStep);
        }
    }
}
