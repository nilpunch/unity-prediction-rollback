using UnityEngine;

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

            var worldHistories = new Histories();
            worldHistories.Add(IdGenerator);
            worldHistories.Add(charactersWorld);
            worldHistories.Add(bulletWorld);
            worldHistories.Add(deathSpikeWorld);

            var worldRollbacks = new Rollbacks();
            worldRollbacks.Add(IdGenerator);
            worldRollbacks.Add(charactersWorld);
            worldRollbacks.Add(bulletWorld);
            worldRollbacks.Add(deathSpikeWorld);
            worldRollbacks.Add(bulletsFactory);

            var worldSimulation = new Simulations();
            worldSimulation.Add(charactersWorld);
            worldSimulation.Add(bulletWorld);
            worldSimulation.Add(deathSpikeWorld);

            TimeTravelMachine = new TimeTravelMachine(worldHistories, worldSimulation, worldRollbacks);

            CharacterMovement = new WorldCommandTimeline<CharacterMoveCommand>(
                new PredictionEntityCommandTimelineFactory<CharacterMoveCommand>(
                new CommandRouter<CharacterMoveCommand>(CharacterWorld)));
            CharacterShooting = new WorldCommandTimeline<CharacterShootCommand>(
                new CommandTimelineFactory<CharacterShootCommand>(
                    new CommandRouter<CharacterShootCommand>(CharacterWorld)));

            TimeTravelMachine.AddCommandsTimeline(CharacterMovement);
            TimeTravelMachine.AddCommandsTimeline(CharacterShooting);
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime * _simulationSpeed;

            TimeTravelMachine.FastForwardToTick(CurrentTick);

            Debug.Log(CurrentTick);
        }
    }
}
