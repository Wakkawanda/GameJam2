using Scripts;

namespace States
{
    public abstract class UnitStateBase : IState
    {
        private protected readonly Player Player;
        
        protected UnitStateBase(Player player)
        {
            Player = player;
        }
        
        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}