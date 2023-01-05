using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateManager : MonoBehaviour
{
    AnimBaseState currentState;
    public AnimIdleState IdleState = new AnimIdleState();
    public AnimWalkState WalkState = new AnimWalkState();
    public AnimJumpState JumpState = new AnimJumpState();
    public AnimDanceState DanceState = new AnimDanceState();
    
    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(AnimBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
