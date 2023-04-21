using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RaycastWeaponBase : WeaponBase
{
    [SerializeField]
    private LayerMask m_raycastMask;

    [Header("Debug")]
    [SerializeField]
    private GameObject m_DebugShotMarker;

    private float m_currentAccuracy;

    protected override void SetWeaponValues()
    {
        base.SetWeaponValues();
        m_currentAccuracy = data.baseAccuracy;
        if (data.weaponType == WeaponType.Projectile) data.weaponType = WeaponType.SemiAuto; //stop nonsensical settings from being used in-game
    }

    protected override void Update()
    {
        if(data != null)
        {
            if (m_currentAccuracy < data.baseAccuracy * 0.99)
            {
                m_currentAccuracy = Mathf.Lerp(m_currentAccuracy, data.baseAccuracy, data.accuracyRecoveryPerSecond * Time.deltaTime);
            }
            else m_currentAccuracy = data.baseAccuracy;
            //as soon as the accuracy lerps up to 99% of the original value it just jumps directly so it doesn't just stay in .9999 decimals forever
        }

        base.Update();
    }

    protected override void Shot()
    {
        base.Shot();
        if (data.weaponType == WeaponType.Shotgun)
        {
            for (int i = 0; i < data.bulletsPerShot; i++) SpawnShot();
        }
        else SpawnShot();

        FinishShot();
    }

    void SpawnShot()
    {
        CalculateAccuracy(out Vector3 direction);

        //create ray that marks the bullet's path
        Ray ray = new Ray(m_raycastSpot.position, direction);
        Debug.DrawRay(m_raycastSpot.position, direction * data.weaponRange, Color.green, 10);

        RaycastHit hit;

        //cast the ray
        if (Physics.Raycast(ray, out hit, data.weaponRange, m_raycastMask))
        {
            if (GameManager.Instance.debugMode)
            {
                Instantiate(m_DebugShotMarker, hit.point, Quaternion.identity);
            }

            if (hit.rigidbody) //if the object has physics then push it
            {
                hit.rigidbody.AddForce(ray.direction * data.forceToApply);
            }

            CharacterBase character = hit.collider.GetComponent<CharacterBase>();
            if (character != null)
            {
                character.Hurt(data.damage);
            }
        }

        Instantiate(data.shotLandingParticles, hit.point, Quaternion.identity);
    }
    
    void CalculateAccuracy(out Vector3 direction)
    {
        //calculate modifier
        float accuracyModifier = (100 - m_currentAccuracy) / 1000;
        direction = m_raycastSpot.forward;

        //apply inaccuracy
        direction.x += Random.Range(-accuracyModifier, accuracyModifier);
        direction.y += Random.Range(-accuracyModifier, accuracyModifier);
        direction.z += Random.Range(-accuracyModifier, accuracyModifier);

        //calculate new accuracy after shot
        m_currentAccuracy -= data.accuracyDropPerShot;
        m_currentAccuracy = Mathf.Clamp(m_currentAccuracy, 0, 100);
    }
}