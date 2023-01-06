using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public static BasicSpawner Instance;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private void Awake()
    {
        Instance = this;
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded%runner.Config.Simulation.DefaultPlayers)* 3,0.5f,0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            if (_spawnedCharacters.ContainsKey(player)) return;

            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    //INPUTS
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
       
        if (Input.GetKey(KeyCode.W))
        {
            data.direction += Vector3.forward;
            data.walkPressed = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            data.direction += Vector3.back;
            data.walkPressed = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            data.direction += Vector3.left;
            data.walkPressed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            data.direction += Vector3.right;
            data.walkPressed = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            data.jumpPressed = true;
        }
        if (Input.GetKey(KeyCode.F1))
        {
            data.dancePressed = true;
            data.danceIndex = 0;        
        }
        if (Input.GetKey(KeyCode.F2))
        {
            data.dancePressed = true;
            data.danceIndex = 1;
        }
        if (Input.GetKey(KeyCode.F3))
        {
            data.dancePressed = true;
            data.danceIndex = 2;
        }

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
       
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    private NetworkRunner _runner;
    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            //Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        }
        );
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0,0,200,40),"Host" ))
            {
                StartGame(GameMode.Host);
            }
            
            if (GUI.Button(new Rect(0,40,200,40),"Join" ))
            {
                StartGame(GameMode.Client);
            }
        }
        if (GUI.Button(new Rect(0,90,200,40),"Next Room"))
        {
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            //_runner.SetActiveScene(SceneManager.GetActiveScene().buildIndex +1);
            OnPlayerJoined(NetworkRunner.GetRunnerForScene(SceneManager.GetActiveScene()), new PlayerRef());
        }
        
    }
}
