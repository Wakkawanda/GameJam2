namespace States
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public virtual void ChangeState(IState newState)
        {
            CurrentState?.OnExit();

            CurrentState = newState;
            CurrentState?.OnEnter();
        }

        public virtual void OnUpdate()
        {
            CurrentState?.OnUpdate();
        }
    }
}