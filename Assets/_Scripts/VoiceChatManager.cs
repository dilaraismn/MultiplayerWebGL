using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;
using Fusion;


namespace MultiplayerWebGL
{
    public class VoiceChatManager : MonoBehaviour
    {
        private string appID = "7dfcae4fd5d84f55835a32cb6493de5d";
        public static VoiceChatManager Instance;
        private IRtcEngine rtcEngine;
        private string token, channel;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            rtcEngine = IRtcEngine.getEngine(appID);
            rtcEngine.OnJoinChannelSuccess += OnJoinChannelSucces;
            rtcEngine.OnLeaveChannel += OnLeaveChannel;
            rtcEngine.OnError += OnError;
        }

        private void OnError(int error, string msg)
        {
            Debug.Log("Error with Agora:" + msg);
        }

        void OnLeaveChannel(RtcStats stats)
        {
            Debug.Log("Left Channel");
        }

        void OnJoinChannelSucces(string channelname, uint uid, int elapsed)
        {
            Debug.Log("Joined Channel: " + channelname);
        }

        public void JoinRoom()
        {
            rtcEngine.JoinChannelByKey(channelKey: token, channelName: channel);
            //NetworkRunner networkRunner = FindObjectOfType<NetworkRunner>();
            //string sessionName = networkRunner.SessionInfo.Name;
            //rtcEngine.JoinChannel(channel);
            //rtcEngine.JoinChannel(SceneManager.GetActiveScene().name);
            Debug.Log("Agora Joined To Channel");
        }

        public void LeaveRoom()
        {
            rtcEngine.LeaveChannel();
        }

        private void OnDestroy()
        {
            IRtcEngine.Destroy();
        }
    } 
}

