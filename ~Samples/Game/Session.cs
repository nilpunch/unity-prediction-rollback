namespace UPR.Samples
{
    public class Session
    {
        public Session()
        {
            var game = new Game();

            var worldTimeline = new WorldTimeline(game.EntityWorld, game.EntityWorld, game.EntityWorld);

            worldTimeline.RegisterTimeline(
                new CommandTimeline<CharacterMoveCommand>(
                    new CommandRouter<CharacterMoveCommand>(
                        game.EntityWorld)));

            worldTimeline.RegisterTimeline(
                new CommandTimeline<CharacterInventoryCommand>(
                    new CommandRouter<CharacterInventoryCommand>(
                        game.EntityWorld)));
        }
    }
}
