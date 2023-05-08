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
                    new CommandRouter<CharacterMoveCommand>()));
        }
    }

    public class Game : ISimulation, IStateHistory
    {
        private readonly Simulations _simulations;
        private readonly StateHistories _stateHistories;

        public Game()
        {
            _simulations = new Simulations();
            _stateHistories = new StateHistories();

            var characterMoveCommandRouter = new CommandRouter<CharacterMoveCommand>();
            var characterInventoryCommandRouter = new CommandRouter<CharacterInventoryCommand>();

            var idGenerator = new UniqueIdGenerator();

            var character = new Character(idGenerator.Generate());

            characterMoveCommandRouter.AddTarget(character, character.Id);
            characterInventoryCommandRouter.AddTarget(character, character.Id);

            _simulations.AddSimulation(character);
            _stateHistories.AddHistory(character);
        }

        public void StepForward(long currentTick)
        {
            _simulations.StepForward(currentTick);
        }

        public int HistoryLength => _stateHistories.HistoryLength;

        public void SaveStep()
        {
            _stateHistories.SaveStep();
        }

        public void Rollback(int ticks)
        {
            _stateHistories.Rollback(ticks);
        }
    }
}
