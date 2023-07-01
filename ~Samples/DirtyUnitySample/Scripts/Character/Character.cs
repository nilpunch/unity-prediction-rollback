using UnityEngine;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class Character : UnityEntity, ICommandPlayer
    {
        [SerializeField] private Lifetime _lifetime;
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private CharacterShooting _characterShooting;

        private IReadOnlyCommandTimeline<CharacterMoveCommand> _moveCommandTimeline;
        private IReadOnlyCommandTimeline<CharacterShootCommand> _shootCommandTimeline;

        public void InitializeCommandTimelines(
            IReadOnlyCommandTimeline<CharacterMoveCommand> moveCommandTimeline,
            IReadOnlyCommandTimeline<CharacterShootCommand> shootCommandTimeline)
        {
            _moveCommandTimeline = moveCommandTimeline;
            _shootCommandTimeline = shootCommandTimeline;
        }

        public void PlayCommands(int tick)
        {
            if (!_lifetime.IsAlive)
                return;

            if (_moveCommandTimeline.HasCommand(tick))
            {
                var command = _moveCommandTimeline.GetCommand(tick);
                _characterMovement?.SetMoveDirection(command.MoveDirection);
            }

            if (_shootCommandTimeline.HasCommand(tick))
            {
                var command = _shootCommandTimeline.GetCommand(tick);

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
