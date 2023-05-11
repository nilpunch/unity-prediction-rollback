namespace UPR.Samples
{
    public class UniqueIdGenerator : IEntity
    {
        private int _idCounter = 0;

        public UniqueIdGenerator()
        {
            Id = new EntityId(-1);
        }

        public EntityId Id { get; }

        public EntityId Generate()
        {
            int id = _idCounter;
            _idCounter += 1;
            return new EntityId(id);
        }

        public void Rollback(int steps)
        {
            CurrentStep -= steps;
            _idCounter -= steps;
        }

        public int CurrentStep { get; private set; }

        public void SaveStep()
        {
            CurrentStep += 1;
        }

        public void StepForward()
        {
        }
    }
}
