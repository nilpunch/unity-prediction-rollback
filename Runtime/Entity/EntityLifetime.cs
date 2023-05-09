namespace UPR
{
    public struct EntityLifetime
    {
        public EntityLifetime(IEntity entity, int birthTick)
        {
            Entity = entity;
            BirthTick = birthTick;
            DeathTick = int.MaxValue;
        }

        public IEntity Entity { get; }
        public int BirthTick { get; private set; }
        public int DeathTick { get; private set; }

        public void KillAtTick(int tick)
        {
            DeathTick = tick;
        }

        public void BornAtTick(int tick)
        {
            BirthTick = tick;
            DeathTick = int.MaxValue;
        }

        public void Resurrect()
        {
            DeathTick = int.MaxValue;
        }

        public void Unborn()
        {
            BirthTick = int.MaxValue;
            DeathTick = int.MaxValue;
        }

        public bool IsAliveAtTick(int tick)
        {
            return tick >= BirthTick && tick < DeathTick;
        }

        public bool IsBornAtTick(int tick)
        {
            return tick >= BirthTick;
        }

        public bool IsDeadAtTick(int tick)
        {
            return tick >= DeathTick;
        }
    }
}
