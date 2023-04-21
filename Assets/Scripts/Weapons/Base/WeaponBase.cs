using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponItem data;

    public Transform m_particleSpawnSpot;
    public Transform m_raycastSpot; //on raycast weapon types, necessary to calculate where the shot will land. On projectile weapon types, used to calculate target position.
    public CharacterBase weaponOwner;
    public AudioSource audioSource;

    public bool RequireShot { get; set; }
    public bool RequireReload { get; set; }

    protected Animator animator;
    protected CharacterController cc;
    protected FPSCharacterController fpscc;

    protected bool m_canShot;
    protected bool m_stillHolding;
    protected bool m_canReload;
    protected bool m_reloading;
    protected bool m_isDrawn;

    protected float m_timeSinceLastShot;
    protected int m_currentAmmo; //ammo in current clip
    protected int m_reservedAmmo; //ammo in all the other clips
    protected float m_shotCooldown;

    public int[] GetAmmoValues()
    {
        int[] values = new int[2];
        values[0] = m_currentAmmo;
        values[1] = m_reservedAmmo;
        return values;
    }

    public void SetAmmoValues(int[] values)
    {
        m_currentAmmo = values[0];
        m_reservedAmmo = values[1];
        UIManager.Instance.ChangeAmmoText(m_currentAmmo, m_reservedAmmo);

        if (m_currentAmmo == data.clipSize) m_canReload = false;
    }

    public void StopHolding()
    {
        m_stillHolding = false;
    }

    public void SetWeaponData(WeaponItem newData)
    {
        Debug.Log("Setting weapon data for " + newData.name);

        data = newData;
        SetWeaponValues();
    }

    protected virtual void SetWeaponValues()
    {
        m_reservedAmmo = data.clipSize * (data.numberOfClips - 1);
        m_currentAmmo = data.clipSize;
        m_shotCooldown = 1f / data.roundsPerSecond;

        SetInternalBools();
    }

    protected virtual void Start()
    {
        transform.rotation = Quaternion.identity; //make sure weapon's rotation is always properly aligned

        animator = GetComponent<Animator>();
        cc = GetComponentInParent<CharacterController>();
        
        try
        {
            Player p = (Player)weaponOwner;
            if(p != null)
            {
                WeaponManager.Instance.WeaponNotifyInit(gameObject);

                fpscc = GetComponentInParent<FPSCharacterController>();
                if (fpscc != null)
                {
                    fpscc.StartSprint += StartRunningAnim;
                    fpscc.EndSprint += StopRunningAnim;
                }
            }
        }
        catch {}
    }

    protected virtual void OnEnable()
    {
        SetInternalBools();

        if (animator != null)
        {
            SetAnimValues();
        }

        StartCoroutine("Draw");
    }

    void SetAnimValues() //values used by animator to scale each animation's duration depending on weapon's values
    {
        animator.SetFloat("reloadTimeModifier", (4 / data.reloadTime) + 0.1f);
        animator.SetFloat("drawTimeModifier", (1 / data.drawTime) + 0.1f);
        animator.SetFloat("shotTimeModifier", (1 / m_shotCooldown) + 0.1f);
        //add 0.1 to the multiplier to make sure animation will finish -before- the next animation
    }

    void StartRunningAnim()
    {
        animator.SetBool("running", true);
    }

    void StopRunningAnim()
    {
        animator.SetBool("running", false);
    }

    void SetAnimIsRunning(bool value)
    {
        animator.SetBool("running", value);
    }

    void SetInternalBools()
    {
        m_isDrawn = false;
        m_canReload = false;
        m_reloading = false;
    }

    protected virtual void Update()
    {
        if(cc != null) animator.SetFloat("velocity", cc.velocity.magnitude);

        if (m_isDrawn && !m_reloading)
        {
            if (m_canShot)
            {
                if (RequireShot)
                {
                    if (m_currentAmmo > 0)
                    {
                        Shot();
                        m_canReload = true;
                        m_stillHolding = true;
                    }
                    else if (m_reservedAmmo > 0) //auto-reload when trying to shoot with an empty clip
                    {
                        Debug.Log("Reloading...");
                        StartCoroutine("Reload");
                    }
                }
            }
            else if (data.weaponType != WeaponType.Auto) //if weapon isn't auto, wait for player to stop holding before shooting
            {
                m_timeSinceLastShot += Time.deltaTime;

                if (m_timeSinceLastShot > m_shotCooldown && !m_stillHolding)
                {
                    m_canShot = true;
                    m_timeSinceLastShot = 0;
                }
            }
        }

        if (m_canReload && !data.autoReloadAfterShot)
        {
            if (Input.GetButton("Reload") & m_reservedAmmo > 0)
            {
                Debug.Log("Reloading...");
                StartCoroutine("Reload");
            }
        }
    }

    protected virtual IEnumerator Draw()
    {
        while (data == null) yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(data.drawTime);

        m_isDrawn = true;
        animator.SetInteger("state", 0);

        if (m_currentAmmo > 0)
        {
            m_canShot = true;
            if (m_currentAmmo < data.clipSize) m_canReload = true;
            else m_canReload = false;

            Debug.Log("ready to shoot");
        }
        else
        {
            m_canShot = false;

            if (data.autoReloadAfterShot & m_reservedAmmo > 0)
            {
                m_canReload = false;
                StartCoroutine("Reload");
            }
            else
            {
                m_canReload = true;
                Debug.Log("must reload");
            }
        }

        yield break;
    }

    protected virtual IEnumerator Reload()
    {
        //start reloading
        m_canReload = false;
        m_reloading = true;
        RequireReload = false;

        if (data.reloadSound) audioSource.PlayOneShot(data.reloadSound);
        animator.SetInteger("state", 2); //start reload animation

        yield return new WaitForSeconds((3 * data.reloadTime) / 4);
        animator.SetInteger("state", 3); //end reload animation

        m_reservedAmmo += m_currentAmmo - data.clipSize; //return the bullets in the used clip and take a new clip
        if (m_reservedAmmo < 0) //if there's not enough bullets for a full clip (the reserved ammo is in the negatives after taking a full clip)
        {
            m_currentAmmo = data.clipSize + m_reservedAmmo; //the new clip has an amount of rounds equal to the usual clip size minus how many bullets are missing to complete it
            m_reservedAmmo = 0; //set it to 0 instead of leaving a negative number
        }
        else m_currentAmmo = data.clipSize; //if there's enough bullets then there's no problem, we can just take the full clip
        UIManager.Instance.ChangeAmmoText(m_currentAmmo, m_reservedAmmo);

        yield return new WaitForSeconds(data.reloadTime / 4);

        animator.SetInteger("state", 0);

        Debug.Log(m_currentAmmo);

        yield return new WaitForEndOfFrame();
        m_reloading = false;
        m_canShot = true;
        yield break;
    }

    protected virtual void Shot()
    {
        m_canShot = false;

        m_currentAmmo--;
        animator.SetInteger("state", 1);
    }

    protected void FinishShot() //called by all children *after* applying their own shot logic
    {
        UIManager.Instance.ChangeAmmoText(m_currentAmmo, m_reservedAmmo);
        m_canReload = true;
        if (data.fireSound) audioSource.PlayOneShot(data.fireSound);

        if(m_particleSpawnSpot) Instantiate(data.shotParticles, m_particleSpawnSpot.position, m_particleSpawnSpot.rotation, m_particleSpawnSpot);
  
        StartCoroutine("EndShot");
    }

    IEnumerator EndShot()
    {
        yield return new WaitForSeconds(m_shotCooldown);

        if (data.autoReloadAfterShot & m_reservedAmmo > 0) //auto-reload after shot, used by grenades & rocket launcher
        {
            StartCoroutine("Reload");
            m_canReload = false;
        }
        else
        {
            if (data.weaponType == WeaponType.Auto) m_canShot = true;
            animator.SetInteger("state", 0);
        }

        yield break;
    }
}
