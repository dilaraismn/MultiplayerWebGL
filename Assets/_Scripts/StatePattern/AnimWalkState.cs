using Fusion;
using UnityEngine;

public class AnimWalkState : AnimBaseState
{
    public AnimWalkState(Player player, AnimStateManager animStateManager, string animBoolName) : base(player, animStateManager, animBoolName)
    {
    }

    //public override void DoChecks()
    //{
    //    base.DoChecks();
    //}

    //When player is in Walk state
    public override void Enter()
    {
        base.Enter();
    }

    //When player switching from Walk state to another.
    public override void Exit()
    {
        base.Exit();
    }

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();

    //    core.Movement.CheckIfShouldFlip(xInput);

    //    core.Movement.SetVelocityX(playerData.movementVelocity * xInput);

    //    if (!isExitingState)
    //    {
    //        if (xInput == 0)
    //        {
    //            stateMachine.ChangeState(player.IdleState);
    //        }
    //        else if (yInput == -1)
    //        {
    //            stateMachine.ChangeState(player.CrouchMoveState);
    //        }
    //    }
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();

    //}
}
