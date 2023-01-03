using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private bool isWalking;
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
        }

        if (Object.IsProxy == true)
        {
            return;
        }

        var input = GetInput<NetworkInputData>();
        if (input.HasValue == true)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    
    public override void Render()
    {
        if (isWalking)
        {
            _animator.SetBool("Walk", true);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }
    }
}
