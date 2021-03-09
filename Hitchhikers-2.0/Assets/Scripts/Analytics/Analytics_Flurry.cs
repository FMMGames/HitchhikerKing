using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlurrySDK;

public class Analytics_Flurry : Analytics_Base
{
    public string ANDROID_API_KEY;
    public string IOS_API_KEY;

    private string FLURRY_API_KEY;

    public override void InitializeAnalytics()
    {
        base.InitializeAnalytics();

        InitializeFlurryAPIKey();
        new Flurry.Builder()
                  .WithCrashReporting(true)
                  .WithLogEnabled(true)
                  .WithLogLevel(Flurry.LogLevel.VERBOSE)
                  .WithMessaging(true)
                  .Build(FLURRY_API_KEY);
    }

    void InitializeFlurryAPIKey()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:       FLURRY_API_KEY = ANDROID_API_KEY;   break;
            case RuntimePlatform.IPhonePlayer:  FLURRY_API_KEY = IOS_API_KEY;       break;
            default:                            FLURRY_API_KEY = null;              break;
        }
    }

    public override void StartAnalytics()
    {
        base.StartAnalytics();
    }

    public override void LogEvent(string eventString, MemorySystemData data, Dictionary<string, object> parameters)
    {
        Flurry.EventRecordStatus status = Flurry.LogEvent(eventString);
    }

    public override void LogLevelStarted(MemorySystemData data)
    {
        base.LogLevelStarted(data);

        LogEvent("Level "+data.level+" started", data, null);
    }

    public override void LogLevelCompleted(MemorySystemData data)
    {
        base.LogLevelCompleted(data);

        LogEvent("Level "+data.level+" completed", data, null);
    }

    public override void LogLevelFailed(MemorySystemData data)
    {
        base.LogLevelFailed(data);

        LogEvent("Level "+data.level+" failed", data, null);
    }

    public void DebugFlurryVersion()
    {
        Debug.Log("AgentVersion: "  + Flurry.GetAgentVersion());
        Debug.Log("ReleaseVersion: "+ Flurry.GetReleaseVersion());
    }
}
