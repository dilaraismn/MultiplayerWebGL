using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;

public class Dilmer : MonoBehaviour
{
    private IRtcEngine mRtcEngine;
    private string token;
    
    
    public void Leave()
    {
        mRtcEngine.LeaveChannel();
        mRtcEngine.DisableAudio();
    }
    
    public void UnloadEngine()
    { 
        
    }
}
