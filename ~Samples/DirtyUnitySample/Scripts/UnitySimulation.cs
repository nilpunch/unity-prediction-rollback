using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class UnitySimulation : MonoBehaviour
    {
        [SerializeField, Range(0.5f, 50f)] private float _simulationSpeed = 1f;
        [SerializeField] private int _ticksPerSecond = 30;
        [SerializeField] private Bullet _bulletPrefab;

        public static SimulationSpeed SimulationSpeed { get; private set; }

        public static TimeTravelMachine TimeTravelMachine { get; private set; }
        public static IWorldCommandTimeline<CharacterMoveCommand> CharacterMovement { get; private set; }
        public static IWorldCommandTimeline<CharacterShootCommand> CharacterShooting { get; private set; }

        public static IEntityWorld<Character> CharacterWorld { get; private set; }
        public static IEntityWorld<Enemy> DeathSpikeWorld { get; private set; }
        public static IEntityWorld<Bullet> BulletsWorld { get; private set; }
        public static IFactory<Bullet> BulletsFactory { get; private set; }

        public static float ElapsedTime { get; set; }

        public static IdGenerator IdGenerator { get; private set; }
        public static int CurrentTick => Mathf.FloorToInt(ElapsedTime * SimulationSpeed.TicksPerSecond);

        private static Rebases s_rebases;
        private static EntityWorld<Character> s_character;

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
            foreach (UnityEntity unityEntity in FindObjectsOfType<UnityEntity>(false))
            {
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
                unityEntity.SaveStep();

                entityIndex += 1;
            }

            IdGenerator = new IdGenerator(entityIndex);

            var bulletsFactory = new EntityFactory<Bullet>(bulletWorld, IdGenerator, new PrefabFactory<Bullet>(_bulletPrefab));
            BulletsFactory = bulletsFactory;

            var worldSimulation = new Simulations();
            worldSimulation.Add(new WorldSimulation<Character>(charactersWorld));
            worldSimulation.Add(new WorldSimulation<Bullet>(bulletWorld));
            worldSimulation.Add(new WorldSimulation<Enemy>(deathSpikeWorld));

            var worldHistories = new Histories();
            worldHistories.Add(IdGenerator);
            worldHistories.Add(new WorldHistory<Character>(charactersWorld));
            worldHistories.Add(new WorldHistory<Bullet>(bulletWorld));
            worldHistories.Add(new WorldHistory<Enemy>(deathSpikeWorld));

            var worldRollbacks = new Rollbacks();
            worldRollbacks.Add(IdGenerator);
            worldRollbacks.Add(new WorldRollback<Character>(charactersWorld));
            worldRollbacks.Add(new WorldRollback<Bullet>(bulletWorld));
            worldRollbacks.Add(new WorldRollback<Enemy>(deathSpikeWorld));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(charactersWorld));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(bulletWorld));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(deathSpikeWorld));
            worldRollbacks.Add(new MispredictionCleanupAfterRollback(bulletsFactory));

            TimeTravelMachine = new TimeTravelMachine(worldHistories, worldSimulation, worldRollbacks);

            CharacterMovement = new WorldCommandTimeline<CharacterMoveCommand>(
                new PredictionCommandTimelineFactory<CharacterMoveCommand>(
                    new CommandRouter<CharacterMoveCommand>(CharacterWorld)));
            CharacterShooting = new WorldCommandTimeline<CharacterShootCommand>(
                new PredictionCommandTimelineFactory<CharacterShootCommand>(
                    new CommandRouter<CharacterShootCommand>(CharacterWorld)));

            TimeTravelMachine.AddCommandsTimeline(CharacterMovement);
            TimeTravelMachine.AddCommandsTimeline(CharacterShooting);
        }

        private void Update()
        {
            ElapsedTime += UnityEngine.Time.deltaTime * _simulationSpeed;

            TimeTravelMachine.FastForwardToTick(CurrentTick);
        }
    }
}
