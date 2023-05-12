using System.Collections.Generic;

namespace UPR.Samples
{
    public class Game
    {
        private readonly IdGenerator _idGenerator = new IdGenerator(0);

        public Game()
        {
            var character = new Character(_idGenerator.Generate());
            EntityWorld.RegisterEntity(character);
        }

        public EntityWorld EntityWorld { get; } = new EntityWorld();
    }
}
