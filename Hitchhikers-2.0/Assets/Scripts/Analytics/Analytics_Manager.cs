using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics_Manager : MonoBehaviour
{
    Analytics_Base[] analyticsReferences;

    void Awake()
    {
        UpdateAnalyticsReferences();
        InitializeAnalytics();
        StartAnalytics();
    }

    void UpdateAnalyticsReferences()
    {
        analyticsReferences = FindObjectsOfType<Analytics_Base>();
    }

    public void InitializeAnalytics()
    {
        foreach (Analytics_Base analytics in analyticsReferences)
        {
            if (analytics)
            {
                analytics.InitializeAnalytics();
            }
        }
    }

    public void StartAnalytics()
    {
        foreach (Analytics_Base analytics in analyticsReferences)
        {
            analytics.StartAnalytics();
        }
    }

    public void LogEvent(string eventName, MemorySystemData data, Dictionary<string, object> parameters)
    {
        if (data!=null)
        {
            foreach (Analytics_Base analytics in analyticsReferences)
            {
                analytics.LogEvent(eventName, data, parameters);
            }
        }
        else {print ("INVALID LOG EVENT DATA! > "+eventName);}
    }

    public void LogLevelStarted(MemorySystemData data)
    {
        if (data!=null)
        {
            foreach (Analytics_Base analytics in analyticsReferences)
            {
                analytics.LogLevelStarted(data);
            }
        }
        else {print ("INVALID LOG EVENT DATA! > LogLevelStarted");}
    }

    public void LogLevelCompleted(MemorySystemData data)
    {
        if (data!=null)
        {
            foreach (Analytics_Base analytics in analyticsReferences)
            {
                analytics.LogLevelCompleted(data);
            }
        }
        else {print ("INVALID LOG EVENT DATA! > LogLevelCompleted");}
    }

    public void LogLevelFailed(MemorySystemData data)
    {
        if (data!=null)
        {
            foreach (Analytics_Base analytics in analyticsReferences)
            {
                analytics.LogLevelFailed(data);
            }
        }
        else {print ("INVALID LOG EVENT DATA! > LogLevelFailed");}
    }
}