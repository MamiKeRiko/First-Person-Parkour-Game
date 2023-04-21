using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseSight : SenseBase
{
    public event SenseEvent SightTriggered;
    public event SenseEvent SightLost;

    public float fieldOfView = 85f;
    public float range = 1000f;

    private void Update()
    {
        if (playerSet && Vector3.Distance(transform.position, m_player.transform.position) < range)
        {
            float angle = Mathf.Abs(Vector3.Angle(transform.forward, (m_player.transform.position - transform.position).normalized));
            if (angle < fieldOfView) OnSense();
            else OnStopSense();
        }
        else OnStopSense();
    }

    protected override void OnSense()
    {
        if (!isSensing)
        {
            SightTriggered();
            base.OnSense();
        }
    }

    protected override void OnStopSense()
    {
        if (isSensing)
        {
            SightLost();
            base.OnStopSense();
        }
    }
}
