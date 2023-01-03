using System;
using com.tvd12.ezyfoxserver.client.logger;
using UnityEngine;

public class UnityLogger : EzySimpleLogger
{
    private readonly object type;
    
    public UnityLogger(object name) : base(name)
    {
    }

    protected override void debug0(string format, params object[] args)
    {
        Debug.Log(type + " - " + format);
    }

    protected override void debug0(string message, Exception e)
    {
        Debug.Log(type + " - " + message + "\n" + e);
    }

    protected override void error0(string format, params object[] args)
    {
        Debug.LogError(type + " - " + format);
    }

    protected override void error0(string message, Exception e)
    {
        Debug.LogError(type + " - " + message + "\n" + e);
    }

    protected override void info0(string format, params object[] args)
    {
        Debug.Log(type + " - " + format);
    }

    protected override void info0(string message, Exception e)
    {
        Debug.Log(type + " - " + message + "\n" + e);
    }

    protected override void trace0(string format, params object[] args)
    {
        Debug.Log(type + " - " + format);
    }

    protected override void trace0(string message, Exception e)
    {
        Debug.Log(type + " - " + message + "\n" + e);
    }

    protected override void warn0(string format, params object[] args)
    {
        Debug.LogWarning(type + " - " + format);
    }

    protected override void warn0(string message, Exception e)
    {
        Debug.LogWarning(type + " - " + message + "\n" + e);
    }
}
