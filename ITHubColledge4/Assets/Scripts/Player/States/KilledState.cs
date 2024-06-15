using Scripts;
using UnityEngine;

namespace States
{
    public class KilledState : UnitStateBase
    {
        public KilledState(Player player) : base(player)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Kill");
            Player.ChangeState(null);
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
        }
    }
}