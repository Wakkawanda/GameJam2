using Scripts;
using UniRx;
using UnityEngine;

namespace States
{
    public class MovingState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;
        private readonly Animator _animator;

        private CompositeDisposable _disposable;
        private Vector2 _direction;

        public MovingState(Player player, PlayerInput playerInput, Animator animator) : base(player)
        {
            _playerInput = playerInput;
            _animator = animator;
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

            if (_animator != null)
            {
                _animator.SetBool("isRun", true);
            }
            Debug.Log("Move");
        }

        public override void OnUpdate()
        {
            _direction = _playerInput.Player.Move.ReadValue<Vector2>();
            
            if (Player == null)
                return;
            
            if (_direction.x < 0)
            {
                Player.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_direction.x > 0)
            {
                Player.transform.localScale = Vector3.one;
            }
            
            if (_playerInput.Player.Move.ReadValue<Vector2>() == Vector2.zero)
            {
                Player.ChangeState(Player.IdleState);
            }
        }

        public override void OnExit()
        {
            _disposable?.Clear();
            _disposable?.Dispose();

            if (_animator != null)
            {
                _animator.SetBool("isRun", false);
            }
        }

        private void Mover()
        {
            if (Player != null)
                Player.Rigidbody2D.MovePosition(Player.Rigidbody2D.position + _direction * Player.Speed * Time.fixedDeltaTime);
        }
    }
}