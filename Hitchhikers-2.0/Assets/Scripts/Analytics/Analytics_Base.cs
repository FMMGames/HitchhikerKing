using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics_Base : MonoBehaviour
{
    [SerializeField] bool debugConsole=true;
    // Start is called before the first frame update
    protected virtual void Start(){}

    // Update is called once per frame
    protected virtual void Update(){}

    public virtual void LogEvent(string eventString, MemorySystemData data, Dictionary<string, object> parameters)
    {
        if (debugConsole) print ("logged '"+eventString+"' on "+AnalyticsName());
    }

    public virtual void InitializeAnalytics()
    {
        if (debugConsole) print ("Initialized "+AnalyticsName());
    }
    
    public virtual void StartAnalytics()
    {
        if (debugConsole) print ("Started "+AnalyticsName());
    }

    public virtual void LogLevelStarted(MemorySystemData data)
    {
        if (debugConsole) print ("logged 'Level Started' on "+AnalyticsName());
    }
    
    public virtual void LogLevelCompleted(MemorySystemData data)
    {
        if (debugConsole) print ("logged 'Level Completed' on "+AnalyticsName());
    }

    public virtual void LogLevelFailed(MemorySystemData data)
    {
        if (debugConsole) print ("logged 'Level Failed' on "+AnalyticsName());
    }

    string AnalyticsName()
    {return this.GetType().Name;}


}
