using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _characterControllerPrototype;

    private void Awake()
    {
        _characterControllerPrototype = GetComponent<NetworkCharacterControllerPrototype>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _characterControllerPrototype.Move(5 * data.direction * Runner.DeltaTime);
        }
    }
}
