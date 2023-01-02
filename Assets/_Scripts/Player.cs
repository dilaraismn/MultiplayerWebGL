using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _characterControllerPrototype;
    private Animator _animator;
    private bool isWalking;

    private void Awake()
    {
        _characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
        _animator = GetComponentInChildren<Animator>();
        isWalking = false;
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterControllerPrototype.Move(5 * data.direction * Runner.DeltaTime);
            isWalking = true;
        }
    }

    private void Update()
    {
        if (isWalking)
        {
            OnWalkChanged(default);
        }
        else
        {
            OnIdleChanged(default);
        }
    }

    public static void OnWalkChanged(Changed<Player> changed)
    {
        changed.Behaviour._animator.Play("Walk");
    }
    public static void OnIdleChanged(Changed<Player> changed)
    {
        changed.Behaviour._animator.Play("Idle");
    }
}
