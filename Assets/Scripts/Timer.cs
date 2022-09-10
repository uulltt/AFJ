using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    float m_StartTime;
    public float maxTime;

    public float elapseTime { get { return Time.time - m_StartTime; } }
    public float elapseAsPercent { get { return elapseTime / maxTime; } }

    public void AddTime(float timeToAdd)
    {
        m_StartTime += timeToAdd;
    }

    public void Reset()
    {
        m_StartTime = Time.time;
    }

    public Timer(float maxTime)
    {
        this.maxTime = maxTime;
    }
}
