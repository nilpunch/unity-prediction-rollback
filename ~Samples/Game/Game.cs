using System.Collections.Generic;

namespace UPR.Samples
{
    public class Game
    {
        private readonly UniqueIdGenerator _idGenerator = new UniqueIdGenerator();

        public Game()
        {
            var character = new Character(_idGenerator.Generate());
            EntityWorld.RegisterEntity(character);
        }

        public EntityWorld EntityWorld { get; } = new EntityWorld();
    }
}
