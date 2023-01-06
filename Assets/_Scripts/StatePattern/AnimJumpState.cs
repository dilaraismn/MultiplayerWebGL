using UnityEngine;

public class AnimJumpState : AnimBaseState
{
    public AnimJumpState(Player player, AnimStateManager animStateManager, string animBoolName) : base(player, animStateManager, animBoolName)
    {
    }

    //When player is in Jump state
    public override void Enter()
    {
        base.Enter();
        player.isWalking = false;
        player.isIdle = false;
        player.isJumping = true;
        Debug.Log("Jump");
    }

    //When player switching from Jump state to another.
    public override void Exit()
    {
        base.Exit();

        
    }

    // Filling this with what you want to add something to FixedUpdate method.
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // Checking isAnimationFinished to switch state when jumping anim is ended.
        if (isAnimationFinished)
        {
            if (player.isWalking)
                animStateManager.SwitchState(player.MoveState);
            else
                animStateManager.SwitchState(player.IdleState);

        }
    }
}
