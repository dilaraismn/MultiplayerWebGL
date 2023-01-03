using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private bool isWalking, isIdle;
    private NetworkCharacterControllerPrototype _characterControllerPrototype;
    private Animator _animator;
    
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
            
            if (data.isPressed)
            {
                isWalking = true;
                isIdle = false;
            }

            if (!data.isPressed)
            {
                isWalking = false;
                isIdle = true;
            }
        }
    }
    
    public override void Render()
    {
        if (isWalking)
        {
            _animator.SetBool("Walk", true);
            _animator.SetBool("Idle", false);
            isIdle = false;
        }

        if (isIdle)
        {
            _animator.SetBool("Walk", false);
            _animator.SetBool("Idle", true);
            isWalking = false;
        }
    }
}
