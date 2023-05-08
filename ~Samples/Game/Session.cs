namespace UPR.Samples
{
    public class Session
    {
        public Session()
        {
            var game = new Game();

            var worldTimeline = new WorldTimeline(game, game);

            worldTimeline.RegisterTimeline(
                new CommandTimeline<CharacterMoveCommand>(
                    new CommandRouter<CharacterMoveCommand>(
                        game.CharacterMoveCommandTargets)));

            worldTimeline.RegisterTimeline(
                new CommandTimeline<CharacterInventoryCommand>(
                    new CommandRouter<CharacterInventoryCommand>(
                        game.CharacterInventoryCommandTargets)));
        }
    }
}
