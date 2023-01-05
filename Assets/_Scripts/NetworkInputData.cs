using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public bool walkPressed;
    public bool jumpPressed;
    public bool dancePressed;
}