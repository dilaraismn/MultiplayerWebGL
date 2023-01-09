using Fusion;
using UnityEditorInternal;
using UnityEngine;

public class AnimIdleState : AnimBaseState
{
    //public override void EnterState(AnimStateManager animation)
    //{
    //}

    //public override void UpdateState(AnimStateManager animation)
    //{
    //}

    //public override void ExitState(AnimStateManager animation)
    //{
    //    animation.SwitchState(animation.WalkState);
    //}

    public AnimIdleState(Player player, AnimStateManager animStateManager, string animBoolName) : base(player, animStateManager, animBoolName)
    {

    }

    //When player is in Idle state
    public override void Enter()
    {
        base.Enter();
        Debug.Log("idle");
    }

    //When player switching from Idle state to another.
    public override void Exit()
    {
        base.Exit();
    }

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();

    //    if (!isExitingState)
    //    {
    //        if (xInput != 0)
    //        {
    //            stateMachine.ChangeState(player.MoveState);
    //        }
    //        else if (yInput == -1)
    //        {
    //            stateMachine.ChangeState(player.CrouchIdleState);
    //        }
    //    }

    //}

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();

    //    xInput = player.InputHandler.NormInputX;
    //    yInput = player.InputHandler.NormInputY;
    //    JumpInput = player.InputHandler.JumpInput;
    //    grabInput = player.InputHandler.GrabInput;
    //    dashInput = player.InputHandler.DashInput;

    //    if (player.InputHandler.AttackInputs[(int)CombatInputs.primary] && !isTouchingCeiling)
    //    {
    //        stateMachine.ChangeState(player.PrimaryAttackState);
    //    }
    //    else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
    //    {
    //        stateMachine.ChangeState(player.SecondaryAttackState);
    //    }
    //    else if (JumpInput && player.JumpState.CanJump())
    //    {
    //        stateMachine.ChangeState(player.JumpState);
    //    }
    //    else if (!isGrounded)
    //    {
    //        player.InAirState.StartCoyoteTime();
    //        stateMachine.ChangeState(player.InAirState);
    //    }
    //    else if (isTouchingWall && grabInput && isTouchingLedge)
    //    {
    //        stateMachine.ChangeState(player.WallGrabState);
    //    }
    //    else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling)
    //    {
    //        stateMachine.ChangeState(player.DashState);
    //    }
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //}
}
