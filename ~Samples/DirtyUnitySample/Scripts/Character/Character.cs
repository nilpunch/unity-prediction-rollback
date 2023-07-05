using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class Character : UnityEntity, ICommandPlayer
    {
        [SerializeField] private Lifetime _lifetime;
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private CharacterShooting _characterShooting;

        public ICommandTimeline<CharacterMoveCommand> MoveCommandTimeline { get; private set; }
        public ICommandTimeline<CharacterShootCommand> ShootCommandTimeline { get; private set; }

        public void InitializeCommandTimelines(
            ICommandTimeline<CharacterMoveCommand> moveCommandTimeline,
            ICommandTimeline<CharacterShootCommand> shootCommandTimeline)
        {
            MoveCommandTimeline = moveCommandTimeline;
            ShootCommandTimeline = shootCommandTimeline;
        }

        public void PlayCommands(int tick)
        {
            if (!_lifetime.IsAlive)
                return;

            if (MoveCommandTimeline.HasCommand(tick))
            {
                var command = MoveCommandTimeline.GetCommand(tick);
                _characterMovement?.SetMoveDirection(command.MoveDirection);
            }

            if (ShootCommandTimeline.HasCommand(tick))
            {
                var command = ShootCommandTimeline.GetCommand(tick);

                _characterShooting.SetShootingDirection(command.Direction);
                if (command.IsShooting)
                {
                    _characterShooting.EnableShooting();
                }
                else
                {
                    _characterShooting.DisableShooting();
                }
            }
        }
    }
}
