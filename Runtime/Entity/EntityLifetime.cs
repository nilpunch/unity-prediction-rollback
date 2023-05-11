namespace UPR
{
    public struct EntityLifetime
    {
        public EntityLifetime(IEntity entity, int birthStep)
        {
            Entity = entity;
            BirthStep = birthStep;
            DeathStep = int.MaxValue;
        }

        public IEntity Entity { get; }
        public int BirthStep { get; private set; }
        public int DeathStep { get; private set; }

        public void KillAtStep(int tick)
        {
            DeathStep = tick;
        }

        public void BornAtTick(int tick)
        {
            BirthStep = tick;
            DeathStep = int.MaxValue;
        }

        public void Resurrect()
        {
            DeathStep = int.MaxValue;
        }

        public void Unborn()
        {
            BirthStep = int.MaxValue;
            DeathStep = int.MaxValue;
        }

        public bool IsAliveAtStep(int tick)
        {
            return tick >= BirthStep && tick < DeathStep;
        }

        public bool IsDeadAtTick(int tick)
        {
            return tick >= DeathStep;
        }

        public bool IsBornAtStep(int tick)
        {
            return tick == BirthStep;
        }
    }
}
