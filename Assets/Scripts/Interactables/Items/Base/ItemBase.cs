using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : TriggerObjectBase
{
    public string m_itemName;

    public bool m_expiresImmediately;
    public float m_expiresWithTime;

    public bool m_applyRepeated;
    public float m_applyRepeatedInterval;
    public int m_applyRepeatedTimes;

    public GameObject m_effect;
    public AudioClip m_pickupSound;

    AudioSource source;
    Renderer render;
    Collider col;

    protected Player m_player;

    bool interruptEffect = false;

    public void InterruptItemEffect()
    {
        interruptEffect = true;
    }

    private void Start()
    {
        render = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    public override void OnTriggerWithPlayer(Player player)
    {
        m_player = player;
        source = player.GetComponent<AudioSource>();

        if (m_effect != null)
        {

        }
        if (source != null)
        {

        }

        render.enabled = false;
        col.enabled = false;

        if (m_applyRepeated) StartCoroutine("ApplyEffectRepeating");
        else ApplyEffect();
    }

    protected virtual void ApplyEffect()
    {
        if (!m_applyRepeated)
        {
            if (m_expiresImmediately) ExitAction();
            else Invoke("ExitAction", m_expiresWithTime);
        }
    }

    IEnumerator ApplyEffectRepeating()
    {
        int i = 0;
        while (!interruptEffect & i < m_applyRepeatedTimes)
        {
            ApplyEffect();
            yield return new WaitForSeconds(m_applyRepeatedInterval);
            i++;
        }
        ExitAction();
        yield break;
    }

    protected virtual void ExitAction()
    {
        if(m_effect != null)
        {

        }
        if(source != null)
        {
            source.Stop();
        }

        Destroy(gameObject);
    }
}
