using UnityEngine;

public abstract class AnimBaseState
{
  public abstract void EnterState(AnimStateManager animation);

  public abstract void UpdateState(AnimStateManager animation);

   public abstract void ExitState(AnimStateManager animation);

}
