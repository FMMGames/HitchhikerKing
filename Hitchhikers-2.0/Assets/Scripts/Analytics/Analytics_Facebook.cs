using UnityEngine;
using Facebook.Unity;

using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Analytics_Facebook : Analytics_Base
{
    public override void InitializeAnalytics()
    {
        base.InitializeAnalytics();

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {Application.targetFrameRate = 60;}

        FB.Init(FBInitCallBack);
    }

    public override void LogEvent(string eventString, MemorySystemData data, Dictionary<string, object> parameters)
    {
        base.LogEvent(eventString, data, parameters);

        parameters[AppEventParameterName.ContentID] = eventString;
        parameters[AppEventName.AchievedLevel] = data.level.ToString();

        FB.LogAppEvent(eventString, data.level, parameters);
    }

    public override void LogLevelStarted(MemorySystemData data)
    {
        base.LogLevelStarted(data);

        FB.LogAppEvent("Level_Started", data.level, Parameters(data));
    }

    public override void LogLevelCompleted(MemorySystemData data)
    {
        base.LogLevelCompleted(data);

        FB.LogAppEvent("Level_Complete", data.level, Parameters(data));
    }

    public override void LogLevelFailed(MemorySystemData data)
    {
        base.LogLevelFailed(data);

        FB.LogAppEvent("Level_Failed", data.level, Parameters(data));
    }

    private void FBInitCallBack()
    {
        if (FB.IsInitialized)
            FB.ActivateApp();
    }

    public void OnApplicationPause(bool paused)
    {
        if (!paused)
            if (FB.IsInitialized)
                FB.ActivateApp();
    }

    Dictionary<string, object> Parameters(MemorySystemData data)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.ContentID] = "Level";
        parameters[AppEventName.AchievedLevel] = data.level.ToString();

        return parameters;
    }
}