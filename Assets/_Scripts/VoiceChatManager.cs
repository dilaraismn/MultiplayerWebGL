using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.Android;

namespace MultiplayerWebGL
{
    public class VoiceChatManager : MonoBehaviour
    {
        private string appID = "b4bb63f84ac64b34913d70e5034fb4d3";
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
            rtcEngine.EnableAudio();
            //rtcEngine.JoinChannelByKey(token, channel);
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
            //options.autoSubscribeAudio = true;
            Debug.Log("Publishing Audio");
        }

        public void StopPublishingAudio()
        {
            var options = new ChannelMediaOptions();
            options.publishLocalAudio = false;
            //options.autoSubscribeAudio = false;
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

    internal class RtcEngineEventHandler : AgoraWebGLEventHandler
    {
        private readonly VoiceChatManager _audioSample;

        internal RtcEngineEventHandler(VoiceChatManager audioSample)
        {
            _audioSample = audioSample;
        }
        public void OnJoinChannelSuccess(IRtcEngine connection, int elapsed)
        {
            Debug.Log("OnJoinChannelSuccess");
        }

        public void OnLeaveChannel(IRtcEngine connection, RtcStats stats)
        {
            Debug.Log("OnJoinLeavelSuccess");
        }
    }
}