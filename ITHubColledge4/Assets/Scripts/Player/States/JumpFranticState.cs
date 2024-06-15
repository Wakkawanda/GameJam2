using Scripts;
using UnityEngine;

namespace States
{
    public class JumpFranticState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;

        public JumpFranticState(Player player, PlayerInput playerInput) : base(player)
        {
            _playerInput = playerInput;
        }

        public override void OnEnter()
        {
            //todo animation attack
            Debug.Log("JumpFranticState");
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