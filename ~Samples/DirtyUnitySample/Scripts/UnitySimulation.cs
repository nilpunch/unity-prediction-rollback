using UnityEngine;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField] private int _ticksPerSecond = 30;
        [SerializeField] private Bullet _bulletPrefab;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static TimeTravelMachine TimeTravelMachine { get; private set; }
        public static ICommandTimeline<CharacterMoveCommand> CharacterMovement { get; private set; }
        public static ICommandTimeline<CharacterShootCommand> CharacterShooting { get; private set; }

        public static IEntityWorld<Character> CharacterWorld { get; private set; }
        public static IEntityWorld<Enemy> DeathSpikeWorld { get; private set; }
        public static IEntityWorld<Bullet> BulletsWorld { get; private set; }

        public static IFactory<Bullet> BulletsFactory { get; private set; }

        public static float ElapsedTime { get; set; }

        public static IdGenerator IdGenerator { get; private set; }
        public static int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private void Start()
        {
            Physics.autoSyncTransforms = true;

            SimulationSpeed = new SimulationSpeed(_ticksPerSecond);

            var charactersWorld = new EntityWorld<Character>();
            CharacterWorld = charactersWorld;
            var bulletWorld = new EntityWorld<Bullet>();
            BulletsWorld = bulletWorld;
            var deathSpikeWorld = new EntityWorld<Enemy>();
            DeathSpikeWorld = deathSpikeWorld;

            int entityIndex = 0;
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>())
            {
                unityEntity.ResetLife();

                switch (unityEntity)
                {
                    case Character character:
                        CharacterWorld.RegisterEntity(character, new EntityId(entityIndex));
                        break;
                    case Enemy deathSpike:
                        DeathSpikeWorld.RegisterEntity(deathSpike, new EntityId(entityIndex));
                        break;
                }

                // Made entity persistent
                unityEntity.SubmitStep();

                entityIndex += 1;
            }

            IdGenerator = new IdGenerator(entityIndex);

            var bulletsFactory = new EntityFactory<Bullet>(bulletWorld, IdGenerator, new PrefabFactory<Bullet>(_bulletPrefab));
            BulletsFactory = bulletsFactory;

            var worldReversibleHistory = new ReversibleHistories();
            worldReversibleHistory.AddHistory(new ReversibleHistory(IdGenerator, IdGenerator));
            worldReversibleHistory.AddHistory(new ReversibleHistory(charactersWorld, charactersWorld));
            worldReversibleHistory.AddHistory(new ReversibleHistory(bulletWorld, bulletWorld));
            worldReversibleHistory.AddHistory(new ReversibleHistory(deathSpikeWorld, deathSpikeWorld));
            worldReversibleHistory.AddHistory(new ReversibleHistory(bulletsFactory, bulletsFactory));

            var worldRollbacks = new Rollbacks();
            worldRollbacks.AddRollback(worldReversibleHistory);

            var worldSimulation = new Simulations();
            worldSimulation.AddSimulation(charactersWorld);
            worldSimulation.AddSimulation(bulletWorld);
            worldSimulation.AddSimulation(deathSpikeWorld);

            TimeTravelMachine = new TimeTravelMachine(worldReversibleHistory, worldSimulation, worldRollbacks);

            CharacterMovement = new CommandTimeline<CharacterMoveCommand>(
                new CommandRouter<CharacterMoveCommand>(CharacterWorld));
            CharacterShooting = new CommandTimeline<CharacterShootCommand>(
                new CommandRouter<CharacterShootCommand>(CharacterWorld));

            TimeTravelMachine.AddCommandsTimeline(CharacterMovement);
            TimeTravelMachine.AddCommandsTimeline(CharacterShooting);
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;

            TimeTravelMachine.FastForwardToTick(CurrentTick);
        }
    }
}
