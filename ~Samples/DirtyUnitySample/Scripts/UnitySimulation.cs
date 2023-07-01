using UnityEngine;
using UPR.Networking;
using UPR.PredictionRollback;
using UPR.Useful;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 50f)] private float _simulationSpeed = 1f;
        [SerializeField] private int _ticksPerSecond = 30;
        [SerializeField] private Bullet _bulletPrefab;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static WorldTimeline WorldTimeline { get; private set; }

        public static TargetRegistry<ICommandTimeline<CharacterMoveCommand>> MoveCommandTimelineRegistery { get; private set; }
        public static TargetRegistry<ICommandTimeline<CharacterShootCommand>> ShootCommandTimelineRegistery { get; private set; }
        public static ITargetRegistry<Character> CharacterRegistry { get; private set; }
        public static ITargetRegistry<Enemy> DeathSpikeRegistry { get; private set; }
        public static ITargetRegistry<Bullet> BulletsRegistry { get; private set; }
        public static EntityFactory<Bullet> BulletsFactory { get; private set; }

        public static float ElapsedTime { get; set; }

        public static IdGenerator IdGenerator { get; private set; }
        public static TickCounter WorldTickCounter { get; private set; }
        public static int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private static Rebases Rebases { get; set; }
        public static RebaseCounter RebaseCounter { get; set; }

        private void Start()
        {
            Application.targetFrameRate = 0;
            QualitySettings.vSyncCount = 0;

            Physics.autoSyncTransforms = true;

            SimulationSpeed = new SimulationSpeed(_ticksPerSecond);

            var characterRegistry = new TargetRegistry<Character>();
            CharacterRegistry = characterRegistry;
            var bulletRegistry = new TargetRegistry<Bullet>();
            BulletsRegistry = bulletRegistry;
            var enemyRegistry = new TargetRegistry<Enemy>();
            DeathSpikeRegistry = enemyRegistry;

            MoveCommandTimelineRegistery = new TargetRegistry<ICommandTimeline<CharacterMoveCommand>>();
            ShootCommandTimelineRegistery = new TargetRegistry<ICommandTimeline<CharacterShootCommand>>();

            int entityIndex = 0;
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>(false))
            {
                switch (unityEntity)
                {
                    case Character character:
                        var moveCommandTimeline = new FadeOutPrediction<CharacterMoveCommand>(new CommandTimeline<CharacterMoveCommand>(), 30, 30);
                        var shootCommandTimeline = new RepeatPrediction<CharacterShootCommand>(new CommandTimeline<CharacterShootCommand>());
                        character.InitializeCommandTimelines(moveCommandTimeline, shootCommandTimeline);

                        MoveCommandTimelineRegistery.Add(moveCommandTimeline, new TargetId(entityIndex));
                        ShootCommandTimelineRegistery.Add(shootCommandTimeline, new TargetId(entityIndex));
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
            worldRollbacks.Add(new RollbackMispredictionCleanup(new TargetRegistryCleanup<Character>(characterRegistry)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new TargetRegistryCleanup<Bullet>(bulletRegistry)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new TargetRegistryCleanup<Enemy>(enemyRegistry)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(BulletsFactory));

            var worldCommandPlayers = new CommandPlayers();
            worldCommandPlayers.Add(new CollectionCommandPlayer(characterRegistry));

            WorldTimeline = new WorldTimeline(worldHistories, worldSimulation, worldRollbacks, worldCommandPlayers);

            RebaseCounter = new RebaseCounter(WorldTickCounter);
            Rebases = new Rebases();
            Rebases.Add(new CollectionRebase<Character>(characterRegistry, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Bullet>(bulletRegistry, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Enemy>(enemyRegistry, WorldTickCounter));
            Rebases.Add(RebaseCounter);
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime * _simulationSpeed;

            WorldTimeline.FastForwardToTick(CurrentTick);
        }

        public static void ForgetFromBegin(int steps)
        {
            Rebases.ForgetFromBeginning(Mathf.Max(Mathf.Min(RebaseCounter.StepsSaved, steps), 0));
        }
    }
}
