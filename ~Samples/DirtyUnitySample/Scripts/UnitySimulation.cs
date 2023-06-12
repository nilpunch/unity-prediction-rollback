using UnityEngine;
using UPR.PredictionRollback;
using UPR.Utils;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 50f)] private float _simulationSpeed = 1f;
        [SerializeField] private int _ticksPerSecond = 30;
        [SerializeField] private Bullet _bulletPrefab;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static TimeTravelMachine TimeTravelMachine { get; private set; }

        public static ICommandTargetRegistry<Character> CharacterRegistry { get; private set; }
        public static ICommandTargetRegistry<Enemy> DeathSpikeRegistry { get; private set; }
        public static ICommandTargetRegistry<Bullet> BulletsRegistry { get; private set; }
        public static EntityFactory<Bullet> BulletsFactory { get; private set; }

        public static float ElapsedTime { get; set; }

        public static IdGenerator IdGenerator { get; private set; }
        public static TickCounter WorldTickCounter { get; private set; }
        public static int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private static Rebases Rebases { get; set; }
        private static RebaseCounter RebaseCounter { get; set; }

        private void Start()
        {
            Physics.autoSyncTransforms = true;

            SimulationSpeed = new SimulationSpeed(_ticksPerSecond);

            var characterRegistry = new CommandTargetRegistry<Character>();
            CharacterRegistry = characterRegistry;
            var bulletRegistry = new CommandTargetRegistry<Bullet>();
            BulletsRegistry = bulletRegistry;
            var enemyRegistry = new CommandTargetRegistry<Enemy>();
            DeathSpikeRegistry = enemyRegistry;

            int entityIndex = 0;
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>(false))
            {
                switch (unityEntity)
                {
                    case Character character:
                        CharacterRegistry.Add(character, new TargetId(entityIndex));
                        break;
                    case Enemy deathSpike:
                        DeathSpikeRegistry.Add(deathSpike, new TargetId(entityIndex));
                        break;
                }

                // Made entity persistent
                unityEntity.SaveStep();

                entityIndex += 1;
            }

            IdGenerator = new IdGenerator(entityIndex);
            WorldTickCounter = new TickCounter();
            BulletsFactory = new EntityFactory<Bullet>(bulletRegistry, IdGenerator, new PrefabFactory<Bullet>(_bulletPrefab));

            var worldSimulation = new Simulations();
            worldSimulation.Add(WorldTickCounter);
            worldSimulation.Add(new CollectionSimulation(characterRegistry));
            worldSimulation.Add(new CollectionSimulation(bulletRegistry));
            worldSimulation.Add(new CollectionSimulation(enemyRegistry));

            var worldHistories = new Histories();
            worldHistories.Add(IdGenerator);
            worldHistories.Add(new CollectionHistory(characterRegistry));
            worldHistories.Add(new CollectionHistory(bulletRegistry));
            worldHistories.Add(new CollectionHistory(enemyRegistry));

            var worldRollbacks = new Rollbacks();
            worldRollbacks.Add(IdGenerator);
            worldRollbacks.Add(WorldTickCounter);
            worldRollbacks.Add(new CollectionRollback(characterRegistry));
            worldRollbacks.Add(new CollectionRollback(bulletRegistry));
            worldRollbacks.Add(new CollectionRollback(enemyRegistry));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(new TargetRegisterCleanup<Character>(characterRegistry)));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(new TargetRegisterCleanup<Bullet>(bulletRegistry)));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(new TargetRegisterCleanup<Enemy>(enemyRegistry)));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(BulletsFactory));

            var worldCommandsPlayers = new CommandPlayers();
            worldCommandsPlayers.Add(new CollectionCommandPlayer(characterRegistry));

            TimeTravelMachine = new TimeTravelMachine(worldHistories, worldSimulation, worldRollbacks, worldCommandsPlayers);

            RebaseCounter = new RebaseCounter(WorldTickCounter);
            Rebases = new Rebases();
            Rebases.Add(new CollectionRebase<Character>(characterRegistry, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Bullet>(bulletRegistry, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Enemy>(enemyRegistry, WorldTickCounter));
            Rebases.Add(RebaseCounter);
        }

        private void Update()
        {
            ElapsedTime += UnityEngine.Time.deltaTime * _simulationSpeed;

            TimeTravelMachine.FastForwardToTick(CurrentTick);
        }

        public static void ForgetFromBegin(int steps)
        {
            Rebases.ForgetFromBeginning(Mathf.Max(Mathf.Min(RebaseCounter.StepsSaved, steps), 0));
        }
    }
}
