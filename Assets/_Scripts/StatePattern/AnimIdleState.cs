using UnityEngine;

public class AnimIdleState : AnimBaseState
{
    public override void EnterState(AnimStateManager animation)
    {
        //play idle anim
        Debug.Log("hello from dance");
    }

    public override void UpdateState(AnimStateManager animation)
    {
        
    }

    public override void ExitState(AnimStateManager animation)
    {
        animation.SwitchState(animation.WalkState);
    }
}
