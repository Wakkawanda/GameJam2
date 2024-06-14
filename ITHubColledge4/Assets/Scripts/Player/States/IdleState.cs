using Scripts;
using UnityEngine;

namespace States
{
    public class IdleState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;

        public IdleState(Player player, PlayerInput playerInput) : base(player)
        {
            _playerInput = playerInput;
        }

        public override void OnEnter()
        {
            Player.Rigidbody2D.velocity = Vector2.zero;
            Debug.Log("Idle");
        }

        public override void OnUpdate()
        {
            if (_playerInput.Player.Move.ReadValue<Vector2>() != Vector2.zero)
            {
                Player.ChangeState(Player.MovingState);
            }
        }

        public override void OnExit()
        {
            
        }
    }
}