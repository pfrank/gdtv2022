using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
    public State CurrentState { get { return currentState; } }

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    void Update()
    {
        currentState?.Tick();
    }
}
