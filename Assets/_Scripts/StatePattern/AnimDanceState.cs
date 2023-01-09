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
        player.canJump = false;
        player.canDance = false;
        Debug.Log("Dancing");
    }

    //When player switching from Jump state to another.
    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(player.CanDanceHandler());
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Checking isAnimationFinished to switch state when dancing anim is ended.
        if (isAnimationFinished)
        {
            animStateManager.SwitchState(player.IdleState);
            
            player.StartCoroutine(player.CanDanceHandler());
        }
    }
}
