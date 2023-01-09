using UnityEngine;
using Fusion;
using Cinemachine;
using System.Collections;

public class Player : NetworkBehaviour
{
    public AnimStateManager StateMachine { get; private set; }


    // States of the player
    public AnimIdleState IdleState { get; private set; }
    public AnimWalkState MoveState { get; private set; }
    public AnimJumpState JumpState { get; private set; }
    public AnimDanceState DanceState { get; private set; }


    public NetworkCharacterControllerPrototype _characterControllerPrototype;


    public Animator _animator { get; private set; }
    //[Networked(OnChanged = nameof(OnWalkChanged))]
    public bool canJump { get; set; }
    
    public bool canDance { get; set; }
   
    private TickTimer _despawnTimer;


    private void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
        this.canJump = true;
        this.canDance = true;
        
        _characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        _animator = GetComponentInChildren<Animator>();

        StateMachine = new AnimStateManager();

        IdleState = new AnimIdleState(this, StateMachine, "Idle");
        MoveState = new AnimWalkState(this, StateMachine, "Walk");
        JumpState = new AnimJumpState(this, StateMachine, "Jump");
        DanceState = new AnimDanceState(this, StateMachine, "Dance");

        StateMachine.Initialize(IdleState); // Setting player to idle state on Awake.
    }

    public CinemachineVirtualCamera vCam;
    public override void Spawned()
    {
        if (Object.HasStateAuthority == true)
        {
            Debug.Log("Object Has State Authority");
            vCam.Priority--;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            vCam.gameObject.SetActive(false);
        }
    }


     public override void FixedUpdateNetwork()
    {
        if (BasicSpawner.isPlayerLeft && Object.HasStateAuthority)
        {
            Debug.Log("Despawn");
            Runner.Despawn(Object); 
        }
        
        StateMachine.currentState.PhysicsUpdate(); // Calling physicsUpdate that we use it like FixedUpdate in States.

        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterControllerPrototype.Move(5 * data.direction * Runner.DeltaTime);

            if (Object.IsProxy == true) return;
            
            // Checks Walk input to switch state to MoveState
            if (data.walkPressed && StateMachine.currentState.Equals(IdleState))
            {
                StateMachine.SwitchState(MoveState);
            }

            // Checks Walk input, if no input -> switch state to IdleState
            if (!data.walkPressed && StateMachine.currentState.Equals(MoveState))
            {
                StateMachine.SwitchState(IdleState);
            }

            // Checks Jump input to switch state
            if (data.jumpPressed && !StateMachine.currentState.Equals(JumpState) && canJump)
            {
                StateMachine.SwitchState(JumpState);
                _characterControllerPrototype.Jump(); // Apllying Jump function when player is in jumpState.
            }

            // Checks Dance inputs to switch state
            if (data.dancePressed && !StateMachine.currentState.Equals(DanceState) && canDance)
            {
                Debug.Log("danceIndex -> " + data.danceIndex);
                StateMachine.SwitchState(DanceState , data.danceIndex);
            }

            // Checks movement and jumping inputs to switch state when the player is dancing.
            if (StateMachine.currentState.Equals(DanceState) && (data.walkPressed /*|| data.jumpPressed*/))
            {
                StateMachine.currentState.isAnimationFinished = true; // Finishes the playing animation.
            }

        }
    }

    // To finishing the current state's active animation when it's called. Usally using in animation events.
    private void AnimationFinishTrigger() => StateMachine.currentState.AnimationFinishTrigger();

    //To Controlling jumping animation finished and waiting 0.1 sec to jump again.
    public IEnumerator CanJumpHandler()
    {
        yield return new WaitForSeconds(0.1f);
        this.canJump = true;
        this.canDance = true;

    }

    public IEnumerator CanDanceHandler()
    {
        yield return new WaitForSeconds(.1f);
        this.canDance = true;
        this.canJump = true;
    }

    #region Old Anim Code
    /*
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
    */
    #endregion
    
}
