using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class Analytics_GameAnalytics : Analytics_Base
{
    public override void InitializeAnalytics()
    {
        base.InitializeAnalytics();
        GameAnalytics.Initialize();
    }

    public override void StartAnalytics()
    {
        base.StartAnalytics();
        //GameAnalytics.
        
    }

    public override void LogLevelStarted(MemorySystemData data)
    {
        base.LogLevelStarted(data);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, data.level.ToString());
    }

    public override void LogLevelCompleted(MemorySystemData data)
    {
        base.LogLevelCompleted(data);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, data.level.ToString());
    }

    public override void LogLevelFailed(MemorySystemData data)
    {
        base.LogLevelFailed(data);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, data.level.ToString());
    }
}
