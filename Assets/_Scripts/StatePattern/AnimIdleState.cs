using UnityEngine;

public class AnimIdleState : AnimBaseState
{
    public override void EnterState(AnimStateManager animation)
    {
        Debug.Log("hello from idle");
    }

    public override void UpdateState(AnimStateManager animation)
    {
        
    }

    public override void ExitState(AnimStateManager animation)
    {
        
    }
}
