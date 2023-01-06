using UnityEngine;

public class AnimDanceState : AnimBaseState
{
    public AnimDanceState(Player player, AnimStateManager animStateManager, string animBoolName) : base(player, animStateManager, animBoolName)
    {

    }

    //When player is in Jump state
    public override void Enter()
    {
        base.Enter();
        player._animator.SetInteger("danceIndex", animStateManager.currentDanceIndex);
        player.isIdle = false;
        player.isWalking = false;
        //player.isDancing = true;
        Debug.Log("Dancing");
    }

    //When player switching from Jump state to another.
    public override void Exit()
    {
        base.Exit();
        //player.isDancing = false;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Checking isAnimationFinished to switch state when dancing anim is ended.
        if (isAnimationFinished)
        {
            if (player.isWalking)
                animStateManager.SwitchState(player.MoveState);
            else
                animStateManager.SwitchState(player.IdleState);
        }
    }
}
