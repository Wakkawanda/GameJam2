using Scripts;
using UnityEngine;

namespace States
{
    public class SmokeState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;

        private Vector2 _direction;

        public SmokeState(Player player, PlayerInput playerInput) : base(player)
        {
            _playerInput = playerInput;
        }

        public override void OnEnter()
        {
            //todo animation attack
            Debug.Log("SmokeState");
            Player.ChangeState(Player.MovingState);
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
        }
    }
}