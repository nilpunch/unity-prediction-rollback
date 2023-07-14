using UnityEngine;
using UPR.Common;
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

        private IContainer<Character> _characters;
        private IContainer<Bullet> _bullets;
        private IContainer<Enemy> _enemies;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static WorldTimeline WorldTimeline { get; private set; }

        public static CommandTimelineRegistry<ICommandTimeline<CharacterMoveCommand>> MoveCommandTimelineRegistery { get; private set; }
        public static CommandTimelineRegistry<ICommandTimeline<CharacterShootCommand>> ShootCommandTimelineRegistery { get; private set; }
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

            _characters = new Container<Character>();
            _bullets = new Container<Bullet>();
            _enemies = new Container<Enemy>();

            MoveCommandTimelineRegistery = new CommandTimelineRegistry<ICommandTimeline<CharacterMoveCommand>>();
            ShootCommandTimelineRegistery = new CommandTimelineRegistry<ICommandTimeline<CharacterShootCommand>>();

            int entityIndex = 0;
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>(false))
            {
                switch (unityEntity)
                {
                    case Character character:
                        var moveCommandTimeline = new CommandTimeline<CharacterMoveCommand>().AppendFadeOutPrediction(30, 30);
                        var shootCommandTimeline = new CommandTimeline<CharacterShootCommand>().AppendRepeatPrediction();
                        character.InitializeCommandTimelines(moveCommandTimeline, shootCommandTimeline);

                        MoveCommandTimelineRegistery.Add(moveCommandTimeline, new CommandTimelineId(entityIndex));
                        ShootCommandTimelineRegistery.Add(shootCommandTimeline, new CommandTimelineId(entityIndex));
                        _characters.Entries.Add(character);
                        break;
                    case Enemy deathSpike:
                        _enemies.Entries.Add(deathSpike);
                        break;
                }

                // Made entity persistent
                unityEntity.SaveStep();

                entityIndex += 1;
            }

            IdGenerator = new IdGenerator(entityIndex);
            WorldTickCounter = new TickCounter();
            BulletsFactory = new EntityFactory<Bullet>(_bullets, new PrefabFactory<Bullet>(_bulletPrefab));

            var worldSimulation = new Simulations();
            worldSimulation.Add(WorldTickCounter);
            worldSimulation.Add(new CollectionSimulation(_characters));
            worldSimulation.Add(new CollectionSimulation(_bullets));
            worldSimulation.Add(new CollectionSimulation(_enemies));

            var worldHistories = new Histories();
            worldHistories.Add(IdGenerator);
            worldHistories.Add(new CollectionHistory(_characters));
            worldHistories.Add(new CollectionHistory(_bullets));
            worldHistories.Add(new CollectionHistory(_enemies));

            var worldRollbacks = new Rollbacks();
            worldRollbacks.Add(IdGenerator);
            worldRollbacks.Add(WorldTickCounter);
            worldRollbacks.Add(new CollectionRollback(_characters));
            worldRollbacks.Add(new CollectionRollback(_bullets));
            worldRollbacks.Add(new CollectionRollback(_enemies));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new CollectionCleanup<Character>(_characters)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new CollectionCleanup<Bullet>(_bullets)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(new CollectionCleanup<Enemy>(_enemies)));
            worldRollbacks.Add(new RollbackMispredictionCleanup(BulletsFactory));

            var worldCommandPlayers = new CommandPlayers();
            worldCommandPlayers.Add(new CollectionCommandPlayer(_characters));

            WorldTimeline = new WorldTimeline(worldHistories, worldSimulation, worldRollbacks, worldCommandPlayers);

            RebaseCounter = new RebaseCounter(WorldTickCounter);
            Rebases = new Rebases();
            Rebases.Add(new CollectionRebase<Character>(_characters, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Bullet>(_bullets, WorldTickCounter));
            Rebases.Add(new CollectionRebase<Enemy>(_enemies, WorldTickCounter));
            Rebases.Add(RebaseCounter);
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime * _simulationSpeed;

            WorldTimeline.UpdateEarliestApprovedTick(Mathf.Max(CurrentTick - _simulateRollbackEveryFrame, 0));
            WorldTimeline.FastForwardToTick(CurrentTick);
        }

        public void ForgetFromBegin(int steps)
        {
            Rebases.ForgetFromBeginning(Mathf.Max(Mathf.Min(RebaseCounter.StepsSaved - _simulateRollbackEveryFrame, steps), 0));
        }

        public int ActiveEntitiesCount()
        {
            int idGenerator = 1;
            int worldCounter = 1;
            return _characters.Entries.Count + _bullets.Entries.Count + _enemies.Entries.Count + idGenerator + worldCounter;
        }
    }
}
