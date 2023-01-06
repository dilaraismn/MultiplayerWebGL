using UnityEngine;

public abstract class AnimBaseState
{
    protected AnimStateManager animStateManager;
    protected Player player;
    protected string animBoolName;

    public bool isAnimationFinished;

    //Constructor of the AnimBaseState class
    public AnimBaseState(Player player, AnimStateManager animStateManager, string animBoolName)
    {
        this.player = player;
        this.animStateManager = animStateManager;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() // When the state is active, this function will work firstly.
    {
        isAnimationFinished = false; // setting the isAnimationFinished to false to activate the current state's anim.
        player._animator.SetBool(animBoolName, true); // Activating the current state's anim. 
    }

    public virtual void Exit() // When the state is switch to another, this function will work.
    {
        player._animator.SetBool(animBoolName, false); // Deactivating the current state's anim. 
    }

    // To finishing the current state's active animation when it's called. Usally using in animation events.
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public virtual void LogicUpdate()   // Filling this with what you want to add something to Update method.
    {

    }

    public virtual void PhysicsUpdate() // Filling this with what you want to add something to FixedUpdate method.
    {

    }


  //  public abstract void EnterState(AnimStateManager animation);

  //public abstract void UpdateState(AnimStateManager animation);

  // public abstract void ExitState(AnimStateManager animation);

}
