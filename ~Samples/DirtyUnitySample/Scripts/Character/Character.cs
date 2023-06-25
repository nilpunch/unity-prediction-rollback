using UnityEngine;
using UPR.Networking;
using UPR.PredictionRollback;

namespace UPR.Samples
{
    public class Character : UnityEntity, ICommandPlayer,
        ICommandTarget<CharacterMoveCommand>,
        ICommandTarget<CharacterShootCommand>
    {
        [SerializeField] private Lifetime _lifetime;
        [SerializeField] private CharacterMovement _characterMovement;
        [SerializeField] private CharacterShooting _characterShooting;

        private ICommandTimeline<CharacterMoveCommand> _moveCommandTimeline;
        private ICommandTimeline<CharacterShootCommand> _shootCommandTimeline;

        ICommandTimeline<CharacterMoveCommand> ICommandTarget<CharacterMoveCommand>.CommandTimeline => _moveCommandTimeline;

        ICommandTimeline<CharacterShootCommand> ICommandTarget<CharacterShootCommand>.CommandTimeline => _shootCommandTimeline;

        public override void Initialize()
        {
            base.Initialize();

            _moveCommandTimeline = new DecayPrediction<CharacterMoveCommand>(new CommandTimeline<CharacterMoveCommand>());
            _shootCommandTimeline = new RepeatPrediction<CharacterShootCommand>(new CommandTimeline<CharacterShootCommand>());
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
