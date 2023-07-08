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

        [Space, SerializeField] private int _simulateRollbackEveryFrame = 8;

        private TargetRegistry<Character> _characterRegistry;
        private TargetRegistry<Bullet> _bulletRegistry;
        private TargetRegistry<Enemy> _enemyRegistry;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static WorldTimeline WorldTimeline { get; private set; }

        public static TargetRegistry<ICommandTimeline<CharacterMoveCommand>> MoveCommandTimelineRegistery { get; private set; }
        public static TargetRegistry<ICommandTimeline<CharacterShootCommand>> ShootCommandTimelineRegistery { get; private set; }
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

            _characterRegistry = new TargetRegistry<Character>();
            _bulletRegistry = new TargetRegistry<Bullet>();
            _enemyRegistry = new TargetRegistry<Enemy>();

            MoveCommandTimelineRegistery = new TargetRegistry<ICommandTimeline<CharacterMoveCommand>>();
            ShootCommandTimelineRegistery = new TargetRegistry<ICommandTimeline<CharacterShootCommand>>();

            int entityIndex = 0;
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>(false))
            {
                switch (unityEntity)
                {
                    case Character character:
                        var moveCommandTimeline = new CommandTimeline<CharacterMoveCommand>().AppendFadeOutPrediction(30, 30);
                        var shootCommandTimeline = new CommandTimeline<CharacterShootCommand>().AppendRepeatPrediction();
                        character.InitializeCommandTimelines(moveCommandTimeline, shootCommandTimeline);

                        MoveCommandTimelineRegistery.Add(moveCommandTimeline, new TargetId(entityIndex));
                        ShootCommandTimelineRegistery.Add(shootCommandTimeline, new TargetId(entityIndex));
                        _characterRegistry.Add(character, new TargetId(entityIndex));
                        break;
                    case Enemy deathSpike:
                        _enemyRegistry.Add(deathSpike, new TargetId(entityIndex));
                        break;
                }

                // Made entity persistent
                unityEntity.SaveStep();

                entityIndex += 1;
            }

            IdGenerator = new IdGenerator(entityIndex);
            WorldTickCounter = new TickCounter();
            BulletsFactory = new EntityFactory<Bullet>(_bulletRegistry, IdGenerator, new PrefabFactory<Bullet>(_bulletPrefab));

            var worldSimulation = new Simulations();
            worldSimulation.Add(WorldTickCounter);
            worldSimulation.Add(new CollectionSimulation(_characterRegistry));
            worldSimulation.Add(new CollectionSimulation(_bulletRegistry));
            worldSimulation.Add(new CollectionSimulation(_enemyRegistry));

            var worldHistories = new Histories();
            worldHistories.Add(IdGenerator);
            worldHistories.Add(new CollectionHistory(_characterRegistry));
            worldHistories.Add(new CollectionHistory(_bulletRegistry));
            worldHistories.Add(new CollectionHistory(_enemyRegistry));

            var worldRollbacks = new Rollbacks();
            worldRollbacks.Add(IdGenerator);
            worldRollbacks.Add(WorldTickCounter);
            worldRollbacks.Add(new CollectionRollback(_characterRegistry));
            worldRollbacks.Add(new CollectionRollback(_bulletRegistry));
            worldRollbacks.Add(new CollectionRollback(_enemyRegistry));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new TargetRegistryCleanup<Character>(_characterRegistry)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new TargetRegistryCleanup<Bullet>(_bulletRegistry)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new TargetRegistryCleanup<Enemy>(_enemyRegistry)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(BulletsFactory));

            var worldCommandPlayers = new CommandPlayers();
            worldCommandPlayers.Add(new CollectionCommandPlayer(_characterRegistry));

            WorldTimeline = new WorldTimeline(worldHistories, worldSimulation, worldRollbacks, worldCommandPlayers);

            RebaseCounter = new RebaseCounter(WorldTickCounter);
            Rebases = new Rebases();
            Rebases.Add(new CollectionRebase<Character>(_characterRegistry, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Bullet>(_bulletRegistry, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Enemy>(_enemyRegistry, WorldTickCounter));
            Rebases.Add(RebaseCounter);
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime * _simulationSpeed;

            WorldTimeline.UpdateEarliestApprovedTick(Mathf.Max(CurrentTick - _simulateRollbackEveryFrame, 0));
            WorldTimeline.FastForwardToTick(CurrentTick);
        }

        public static void ForgetFromBegin(int steps)
        {
            Rebases.ForgetFromBeginning(Mathf.Max(Mathf.Min(RebaseCounter.StepsSaved, steps), 0));
        }

        public int ActiveEntitiesCount()
        {
            int idGenerator = 1;
            int worldCounter = 1;
            return _characterRegistry.Entries.Count + _bulletRegistry.Entries.Count + _enemyRegistry.Entries.Count + idGenerator + worldCounter;
        }
    }
}
