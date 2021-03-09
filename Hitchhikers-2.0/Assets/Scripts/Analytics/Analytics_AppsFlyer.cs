using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class Analytics_AppsFlyer : Analytics_Base, IAppsFlyerConversionData
{

    // These fields are set from the editor so do not modify!
    //******************************//
    public string devKey;
    public string appID;
    public bool isDebug;
    public bool getConversionData;
    //******************************//

    public override void InitializeAnalytics()
    {
        base.InitializeAnalytics();

        // These fields are set from the editor so do not modify!
        //******************************//
        AppsFlyer.setIsDebug(isDebug);
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
        //******************************//
    }

    public override void StartAnalytics()
    {
        base.StartAnalytics();
        
        AppsFlyer.startSDK();
    }

    public override void LogLevelStarted(MemorySystemData data)
    {
        base.LogLevelStarted(data);

        AppsFlyer.sendEvent("Level_Start", GetAppsFlyerEventDictionary(data));
    }

    public override void LogLevelCompleted(MemorySystemData data)
    {
        base.LogLevelCompleted(data);

        AppsFlyer.sendEvent("Level_Complete", GetAppsFlyerEventDictionary(data));
    }

    public override void LogLevelFailed(MemorySystemData data)
    {
        base.LogLevelFailed(data);

        AppsFlyer.sendEvent("Level_Fail", GetAppsFlyerEventDictionary(data));
    }

    Dictionary<string, string> GetAppsFlyerEventDictionary(MemorySystemData data)
    {
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        eventValues.Add("Level", data.level.ToString());
        //eventValues.Add("Jumps", (currentJumps.ToString()+"/"+currentScene.allowedJumps.ToString()));

        return eventValues;
    }

    // Mark AppsFlyer CallBacks
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }
}