using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _characterControllerPrototype;
    private Animator _animator;
    [Networked(OnChanged = nameof(OnWalkChanged))]
    public bool isWalking { get; set; }
    public bool isIdle { get; set; }

    private void Awake()
    {
        _characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        _animator = GetComponentInChildren<Animator>();
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterControllerPrototype.Move(5 * data.direction * Runner.DeltaTime);

            if (Object.IsProxy == true)
            {
                return;
            }
            
            if (data.walkPressed)
            {
                isWalking = true;
                isIdle = false;
            }

            if (!data.walkPressed)
            {
                isWalking = false;
                isIdle = true;
            }
        }
    }

    protected static void OnWalkChanged(Changed<Player> changed)
    {
        changed.LoadNew();
        if (changed.Behaviour.isWalking)
        {
            changed.Behaviour._animator.SetBool("Walk", true);
            changed.Behaviour._animator.SetBool("Idle", false);
            changed.Behaviour.isIdle = false;
            print("Walking");
        }

        if (changed.Behaviour.isIdle)
        {
            changed.Behaviour._animator.SetBool("Walk", false);
            changed.Behaviour._animator.SetBool("Idle", true);
            changed.Behaviour.isWalking = false;
            print("idle");
        }
        changed.LoadNew();
    }
}
