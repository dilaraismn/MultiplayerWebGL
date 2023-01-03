using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora.Rtc;
using Agora_RTC_Plugin;

public class VoiceChatManager : MonoBehaviour
{
    private string appID = "7dfcae4fd5d84f55835a32cb6493de5d";
    private string _channelName = "MyChannel";
    public static VoiceChatManager Instrance;
    private IRtcEngine rtcEngine;
 

    private void Awake()
    {
        if (Instrance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instrance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void SetupVoiceSDKEngine()
    {
        rtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext(appID, 0,
            CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
            AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        // Initialize RtcEngine.
        //RtcEngine.Initialize(context);
        //bunu uncommentla
    }
    private void Start()
    {
        
    }
}
