using UnityEngine;
using Fusion;
using Cinemachine;

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
    public bool isWalking { get; set; }
    public bool isIdle { get; set; }
    public bool isJumping { get; set; }
    public bool isDancing { get; set; }


    private void Awake()
    {
        // DontDestroyOnLoad(this.gameObject);
        
        _characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        _animator = GetComponentInChildren<Animator>();

        StateMachine = new AnimStateManager();

        IdleState = new AnimIdleState(this, StateMachine, "Idle");
        MoveState = new AnimWalkState(this, StateMachine, "Walk");
        JumpState = new AnimJumpState(this, StateMachine, "Jump");
        DanceState = new AnimDanceState(this, StateMachine, "Dance");

        StateMachine.Initialize(IdleState); // Setting player to idle state on Awake.
    }

    public Transform followTransform;
    public CinemachineVirtualCamera vCam;
    public override void Spawned()
    {
        if (Object.HasInputAuthority == true)
        {
            Debug.Log("IS PROXY");
            vCam.m_Follow = this.followTransform;
            vCam.m_LookAt = this.followTransform;
            vCam.Priority--;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //DontDestroyOnLoad(this.gameObject);
            vCam.gameObject.SetActive(false);
        }
    }


     public override void FixedUpdateNetwork()
    {
        StateMachine.currentState.PhysicsUpdate(); // Calling physicsUpdate that we use it like FixedUpdate in States.

        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterControllerPrototype.Move(5 * data.direction * Runner.DeltaTime);

            if (Object.IsProxy == true)
            {
                return;
            }

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
            if (data.jumpPressed && !StateMachine.currentState.Equals(JumpState))
            {
                StateMachine.SwitchState(JumpState);
                _characterControllerPrototype.Jump(); // Apllying Jump function when player is in jumpState.
            }

            // Checks Dance inputs to switch state
            if (data.dancePressed && !StateMachine.currentState.Equals(DanceState))
            {
                Debug.Log("danceIndex -> " + data.danceIndex);
                StateMachine.SwitchState(DanceState , data.danceIndex);
            }

            // Checks movement and jumping inputs to switch state when the player is dancing.
            if (StateMachine.currentState.Equals(DanceState) && (data.walkPressed || data.jumpPressed))
            {
                StateMachine.currentState.isAnimationFinished = true; // Finishes the playing animation. 
            }

        }
    }

    // To finishing the current state's active animation when it's called. Usally using in animation events.
    private void AnimationFinishTrigger() => StateMachine.currentState.AnimationFinishTrigger();


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
}
