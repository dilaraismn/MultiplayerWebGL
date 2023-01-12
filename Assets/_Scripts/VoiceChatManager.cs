using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Agora.Rtc;
using Agora.Util;
using Agora_RTC_Plugin.API_Example.Examples.Basic.JoinChannelAudio;
using Fusion;
using Logger = Agora.Util.Logger;

namespace MultiplayerWebGL
{
    public class VoiceChatManager : MonoBehaviour
    {
        [SerializeField] private AppIdInput _appIdInput;
        private string _appID = "";
        public string _token = "";
        private string _channelName = "";

        public Text LogText;
        internal Logger Log;
        internal IRtcEngine RtcEngine = null;

        private IAudioDeviceManager _audioDeviceManager;
        private DeviceInfo[] _audioPlaybackDeviceInfos;
        public Dropdown _audioDeviceSelect;

        public static VoiceChatManager Instance;
        
        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            LoadAssetData();
            if (CheckAppId())
            {
                InitRtcEngine();
                SetBasicConfiguration();
            }
        }

        private void Update()
        {
            PermissionHelper.RequestMicrophontPermission();
        }

        private bool CheckAppId()
        {
            Log = new Logger(LogText);
            return Log.DebugAssert(_appID.Length > 10, "Please fill in your appId");
        }

        //Show data in AgoraBasicProfile
        private void LoadAssetData()
        {
            if (_appIdInput == null) return;
            _appID = _appIdInput.appID;
            _token = _appIdInput.token;
            _channelName = _appIdInput.channelName;
        }

        private void InitRtcEngine()
        {
            RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
            RtcEngineEventHandler handler = new RtcEngineEventHandler(this);
            RtcEngineContext context = new RtcEngineContext(_appID, 0,
                CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
                AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
            RtcEngine.Initialize(context);
            RtcEngine.InitEventHandler(handler);
        }

        private void SetBasicConfiguration()
        {
            RtcEngine.EnableAudio();
            RtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
            RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        }

        #region -- Button Events ---
        public void JoinChannel()
        {
            //NetworkRunner networkRunner = FindObjectOfType<NetworkRunner>();
            //string sessionName = networkRunner.SessionInfo.Name;
            //_channelName = sessionName;
            RtcEngine.JoinChannel(_token, _channelName);
            Debug.Log("Agora Joined To Channel");
            //GetAudioPlaybackDevice();
        }

        public void LeaveChannel()
        {
            RtcEngine.LeaveChannel();
        }

        public void StopPublishAudio()
        {
            var options = new ChannelMediaOptions();
            options.publishMicrophoneTrack.SetValue(false);
            var nRet =  RtcEngine.UpdateChannelMediaOptions(options);
            Debug.Log("Not Publishing Audio");
        }

        public void StartPublishAudio()
        {
            var options = new ChannelMediaOptions();
            options.publishMicrophoneTrack.SetValue(true);
            var nRet = RtcEngine.UpdateChannelMediaOptions(options);
            Debug.Log("Publishing Audio");
        }

        public void GetAudioPlaybackDevice()
        {
            _audioDeviceSelect.ClearOptions();
            _audioDeviceManager = RtcEngine.GetAudioDeviceManager();
            _audioPlaybackDeviceInfos = _audioDeviceManager.EnumeratePlaybackDevices();
            Log.UpdateLog(string.Format("AudioPlaybackDevice count: {0}", _audioPlaybackDeviceInfos.Length));
            for (var i = 0; i < _audioPlaybackDeviceInfos.Length; i++)
            {
                Log.UpdateLog(string.Format("AudioPlaybackDevice device index: {0}, name: {1}, id: {2}", i,
                    _audioPlaybackDeviceInfos[i].deviceName, _audioPlaybackDeviceInfos[i].deviceId));
            }

            _audioDeviceSelect.AddOptions(_audioPlaybackDeviceInfos.Select(w =>
                    new Dropdown.OptionData(
                        string.Format("{0} :{1}", w.deviceName, w.deviceId)))
                .ToList());
        }

        public void SelectAudioPlaybackDevice()
        {
            if (_audioDeviceSelect == null) return;
            var option = _audioDeviceSelect.options[_audioDeviceSelect.value].text;
            if (string.IsNullOrEmpty(option)) return;

            var deviceId = option.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
            var ret = _audioDeviceManager.SetPlaybackDevice(deviceId);
            Log.UpdateLog("SelectAudioPlaybackDevice ret:" + ret + " , DeviceId: " + deviceId);
        }

        #endregion

        private void OnDestroy()
        {
            if (RtcEngine == null) return;
            RtcEngine.InitEventHandler(null);
            RtcEngine.LeaveChannel();
            RtcEngine.Dispose();
        }
    }

    internal class RtcEngineEventHandler : IRtcEngineEventHandler
    {
        private readonly VoiceChatManager _audioSample;

        internal RtcEngineEventHandler(VoiceChatManager audioSample)
        {
            _audioSample = audioSample;
        }
        public override void OnError(int error, string message)
        {
            _audioSample.Log.UpdateLog(string.Format("OnError err: {0}, msg: {1}", error, message));
        }

        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            int build = 0;
            _audioSample.Log.UpdateLog(string.Format("sdk version: ${0}",
                _audioSample.RtcEngine.GetVersion(ref build)));
            _audioSample.Log.UpdateLog(
                string.Format("OnJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}",
                    connection.channelId, connection.localUid, elapsed));
        }

        public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
        {
            _audioSample.Log.UpdateLog("OnLeaveChannel");
        }
        public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole)
        {
            _audioSample.Log.UpdateLog("OnClientRoleChanged");
        }
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            _audioSample.Log.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
        }
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _audioSample.Log.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid,
                (int)reason));
        }
    }
}
