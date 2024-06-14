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
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
        }
    }
}