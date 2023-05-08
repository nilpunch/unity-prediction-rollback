namespace UPR.Samples
{
    public class Game : ISimulation, IStateHistory
    {
        private readonly Simulations _simulations = new Simulations();
        private readonly StateHistories _stateHistories = new StateHistories();
        private readonly UniqueIdGenerator _idGenerator = new UniqueIdGenerator();

        public Game()
        {
            CharacterMoveCommandTargets = new TargetsCollection<CharacterMoveCommand>();
            CharacterInventoryCommandTargets = new TargetsCollection<CharacterInventoryCommand>();

            var character = new Character(_idGenerator.Generate());
            CharacterMoveCommandTargets.AddTarget(character.Id, character);
            CharacterInventoryCommandTargets.AddTarget(character.Id, character);

            _simulations.AddSimulation(character);
            _stateHistories.AddHistory(character);
        }

        public TargetsCollection<CharacterInventoryCommand> CharacterInventoryCommandTargets { get; }

        public TargetsCollection<CharacterMoveCommand> CharacterMoveCommandTargets { get; }

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
