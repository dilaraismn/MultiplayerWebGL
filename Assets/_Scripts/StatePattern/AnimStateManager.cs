using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateManager
{
    public AnimBaseState currentState { get; private set; } // Represents the active state which player in.

    // Holding the given danceIndex when switching state to dance state to determine which animation will play with this index.
    public int currentDanceIndex { get; set; }

    // Using this on awake function to set starting state to current state.
    public void Initialize(AnimBaseState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    // Using this to switch current state to target state.
    public void SwitchState(AnimBaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // Using this to switch current state to dance state. We overriding this method to give extra parameter to check which dance we want to play.
    public void SwitchState(AnimBaseState newState, int danceIndex)
    {
        currentState.Exit();
        currentState = newState;
        currentDanceIndex = danceIndex;
        currentState.Enter();
    }

}
