namespace UPR.Samples
{
    public interface ICachedEntity
    {
        public void ChangeId(EntityId entityId);
        public void ResetHistory();
    }
}
