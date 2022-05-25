public abstract class BasePlayerState : State
{
    protected PlayerStateMachine stateMachine;

    public BasePlayerState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
