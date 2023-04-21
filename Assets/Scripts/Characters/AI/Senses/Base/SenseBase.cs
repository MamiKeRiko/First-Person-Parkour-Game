using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SenseBase : MonoBehaviour
{
    public delegate void SenseEvent();

    protected bool isSensing;
    protected Player m_player;
    protected bool playerSet;

    protected void Start()
    {
        StartCoroutine("GetPlayer");
    }

    IEnumerator GetPlayer()
    {
        while(m_player == null)
        {
            m_player = GameManager.Instance.GetPlayer();
            yield return new WaitForEndOfFrame();
        }
        playerSet = true;
        yield break;
    }

    public bool GetIsSensing() { return isSensing; }

    protected virtual void OnSense()
    {
        isSensing = true;
    }
    protected virtual void OnStopSense()
    {
        isSensing = false;
    }
}
