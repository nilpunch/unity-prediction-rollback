namespace UPR.Samples
{
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

        public void StepForward(float deltaTime)
        {
            _simulations.StepForward(deltaTime);
        }

        public void SaveStep()
        {
            _stateHistories.SaveStep();
        }

        public void Rollback(int steps)
        {
            _stateHistories.Rollback(steps);
        }
    }
}
