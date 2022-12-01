using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoSingletonGlobal<AdsManager>
{
    public void ShowBanner()
    {

    }  
    
    public void HideBanner()
    {

    }
    
    public void ShowInter(System.Action Callback)
    {
        Callback?.Invoke();
    }    

    public void ShowReward(System.Action Callback_Success, System.Action Callback_Miss)
    {
        Callback_Success?.Invoke();
    }    
}
