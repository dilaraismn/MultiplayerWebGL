using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;
using Fusion;
using UnityEngine.Android;
using UnityEngine.UI;

namespace MultiplayerWebGL
{
    public class VoiceChatManager : MonoBehaviour
    {
        private string appID = "7dfcae4fd5d84f55835a32cb6493de5d";
        public IRtcEngine rtcEngine;
        public string token, channel;
        public static VoiceChatManager Instance;
        private IAudioPlaybackDeviceManager _audioDeviceManager;

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

        public void LoadEngine(string appID, string token = null)
        {
            this.token = token;
            rtcEngine = IRtcEngine.getEngine(appID);
        }

        private void Start()
        {
            LoadEngine(appID, token);
            SetBasicConfiguration();
            SetDebugs();
        }

        private void SetBasicConfiguration()
        {
            rtcEngine.EnableAudio();
            rtcEngine.SetChannelProfile(CHANNEL_PROFILE.CHANNEL_PROFILE_COMMUNICATION);
            rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            Permission.RequestUserPermission(Permission.Microphone);
        }

        private void SetDebugs()
        {
            rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
            rtcEngine.OnLeaveChannel += OnLeaveChannel;
            rtcEngine.OnError += OnError;
        }

        #region Button Events
        public void JoinRoom()
        {
            if (rtcEngine == null) return;

            //NetworkRunner networkRunner = FindObjectOfType<NetworkRunner>();
            //string channel = networkRunner.SessionInfo.Name;
            rtcEngine.JoinChannel(token, channel);
            Debug.Log("Agora Joined To Channel");
        }
        
        public void LeaveRoom()
        {
            rtcEngine.LeaveChannel();
            Debug.Log("Leaving Channel");
        }

        public void StartPublishingAudio()
        {
            var options = new ChannelMediaOptions();
            options.publishLocalAudio = true;
            options.autoSubscribeAudio = true;
            Debug.Log("Publishing Audio");
        }

        public void StopPublishingAudio()
        {
            var options = new ChannelMediaOptions();
            options.publishLocalAudio = false;
            options.autoSubscribeAudio = false;
            Debug.Log("Not Publishing Audio");
        }
        
        private void OnDestroy()
        {
            rtcEngine.LeaveChannel();
            if (rtcEngine != null)
            {
                IRtcEngine.Destroy();
                rtcEngine = null;
            }
        }
        #endregion

        #region Debugs
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
        #endregion
    }

    internal class RtcEngineEventHandler : IRtcEngineEventHandler
    {
        
    }
}