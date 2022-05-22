using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PausedGameState : BaseGameState
{
    public PausedGameState(GameStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("GAME PAUSED");
        PauseGameObjects();
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {
        UnpauseGameObjects();
        Debug.Log("GAME UNPAUSED");
    }

    private IEnumerable<IPausable> FindPausableObjects()
    {
        return GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IPausable>();
    }

    private void PauseGameObjects()
    {
        ;
        foreach (IPausable obj in FindPausableObjects())
        {
            obj.Pause();
        }
    }

    private void UnpauseGameObjects()
    {
        foreach (IPausable obj in FindPausableObjects())
        {
            obj.Unpause();
        }
    }
}
