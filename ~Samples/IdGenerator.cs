namespace UPR.Samples
{
    public class UniqueIdGenerator
    {
        private int _id = 0;

        public EntityId Generate()
        {
            int id = _id;
            _id += 1;
            return new EntityId(id);
        }
    }
}
