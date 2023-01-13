using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;
using Fusion;
using UnityEngine.Android;

namespace MultiplayerWebGL
{
    public class OldVoice : MonoBehaviour
    {
        private string appID = "7dfcae4fd5d84f55835a32cb6493de5d";
        public static OldVoice Instance;
        private IRtcEngine rtcEngine;
        private string token;

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
            rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
            rtcEngine.OnLeaveChannel += OnLeaveChannel;
            rtcEngine.OnError += OnError;
            
            rtcEngine.SetLogFilter(LOG_FILTER.INFO);
            rtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_COMMUNICATION);
        }

        private void Update()
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        
        private void OnError(int error, string msg)
        {
            Debug.Log("Error with Agora:" + msg);
        }

        void OnLeaveChannel(RtcStats stats)
        {
            Debug.Log("Left Channel");
        }

        void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
        {
            Debug.Log("Joined Channel: " + channelName);
        }

        public void JoinRoom()
        {
            NetworkRunner networkRunner = FindObjectOfType<NetworkRunner>();
            string channel = networkRunner.SessionInfo.Name;
            rtcEngine.JoinChannel(channel);
            Debug.Log("Agora Joined To Channel");
            
            rtcEngine.EnableAudio();
            var options = new ChannelMediaOptions();
            options.publishLocalAudio = true;
        }

        
        public void LeaveRoom()
        {
            rtcEngine.LeaveChannel();
            rtcEngine.DisableAudio();
            var options = new ChannelMediaOptions();
            options.publishLocalAudio = false;
        }

        private void OnDestroy()
        {
            if (rtcEngine == null) return;
            IRtcEngine.Destroy();
        }
    } 
}
