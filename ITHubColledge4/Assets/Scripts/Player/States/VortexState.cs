using Scripts;
using UnityEngine;

namespace States
{
    public class VortexState : UnitStateBase
    {
        private readonly PlayerInput _playerInput;

        public VortexState(Player player, PlayerInput playerInput) : base(player)
        {
            _playerInput = playerInput;
        }

        public override void OnEnter()
        {
            //todo animation attack
            Debug.Log("VortexState");
            
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