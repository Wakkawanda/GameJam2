using Scripts;
using UniRx;
using UnityEngine;

namespace States
{
    public class MovingState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;
        private readonly Player _player;
        private Vector2 _direction;
        
        private CompositeDisposable _disposable;

        public MovingState(Player player, PlayerInput playerInput) : base(player)
        {
            _player = player;
            _playerInput = playerInput;
        }

        public override void OnEnter()
        {
            _disposable?.Clear();
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            Observable
                .EveryFixedUpdate()
                .Subscribe(_ => Mover())
                .AddTo(_disposable);
            
            Debug.Log("Move");
        }

        public override void OnUpdate()
        {
            _direction = _playerInput.Player.Move.ReadValue<Vector2>();
            
            if (_playerInput.Player.Move.ReadValue<Vector2>() == Vector2.zero)
            {
                Player.ChangeState(Player.IdleState);
            }
        }

        public override void OnExit()
        {
            _disposable?.Clear();
            _disposable?.Dispose();
        }

        private void Mover()
        {
            _player.Rigidbody2D.MovePosition(_player.Rigidbody2D.position + _direction * _player.Speed * Time.fixedDeltaTime);
        }
    }
}