using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics_Base : MonoBehaviour
{
    public bool hasInitialized;
    // Start is called before the first frame update
    protected virtual void Start(){}

    // Update is called once per frame
    protected virtual void Update(){}

    public virtual void LogEvent(string eventString, MemorySystemData data, Dictionary<string, object> parameters)
    {

    }

    public virtual void InitializeAnalytics(){}
    public virtual void StartAnalytics(){}

    public virtual void LogLevelStarted(MemorySystemData data){}
    public virtual void LogLevelCompleted(MemorySystemData data){}
    public virtual void LogLevelFailed(MemorySystemData data){}
}
