using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateManager : MonoBehaviour
{
    AnimBaseState currentState;
    private AnimIdleState IdleState = new AnimIdleState();
    private AnimWalkState WalkState = new AnimWalkState();
    private AnimJumpState JumpState = new AnimJumpState();
    private AnimDanceState DanceState = new AnimDanceState();
    
    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        
    }
}
