using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem
{
    [SerializeField]
    private string m_name;
    public string name { get { return m_name; } }

    [SerializeField]
    private WeaponType m_weaponType;
    public WeaponType weaponType { get { return m_weaponType; } set { m_weaponType = value; } }

    [SerializeField]
    private GameObject m_projectilePrefab;
    public GameObject projectilePrefab { get { return m_projectilePrefab; } }

    [SerializeField]
    private int m_damage = 80;
    public int damage { get { return m_damage; } }

    [SerializeField]
    private float m_forceToApply = 20.0f;
    public float forceToApply { get { return m_forceToApply; } }

    [SerializeField]
    private float m_weaponRange = 9999.0f;
    public float weaponRange { get { return m_weaponRange; } }

    [SerializeField]
    private float m_baseAccuracy = 95; //0 - very inaccurate, 100 - perfect accuracy
    public float baseAccuracy { get { return m_baseAccuracy; } }

    [SerializeField]
    private float m_accuracyDropPerShot = 15;
    public float accuracyDropPerShot { get { return m_accuracyDropPerShot; } }

    [SerializeField]
    private float m_accuracyRecoveryPerSecond = 20;
    public float accuracyRecoveryPerSecond { get { return m_accuracyRecoveryPerSecond; } }

    [SerializeField]
    private int m_bulletsPerShot;
    public int bulletsPerShot { get { return m_bulletsPerShot; } }

    [SerializeField]
    private float m_roundsPerSecond;
    public float roundsPerSecond { get { return m_roundsPerSecond; } }

    [SerializeField]
    private int m_clipSize;
    public int clipSize { get { return m_clipSize; } }

    [SerializeField]
    private int m_numberOfClips;
    public int numberOfClips { get { return m_numberOfClips; } }

    [SerializeField]
    private float m_reloadTime;
    public float reloadTime { get { return m_reloadTime; } }

    [SerializeField]
    private bool m_autoReloadAfterShot;
    public bool autoReloadAfterShot { get { return m_autoReloadAfterShot; } }

    [SerializeField]
    private float m_drawTime;
    public float drawTime { get { return m_drawTime; } }

    [SerializeField]
    private Texture2D m_crosshairTexture;
    public Texture2D crosshairTexture { get { return m_crosshairTexture; } }

    [SerializeField]
    private AudioClip m_fireSound;
    public AudioClip fireSound { get { return m_fireSound; } }

    [SerializeField]
    private AudioClip m_reloadSound;
    public AudioClip reloadSound { get { return m_reloadSound; } }

    [SerializeField]
    private GameObject m_shotParticles;
    public GameObject shotParticles { get { return m_shotParticles; } }

    [SerializeField]
    private Transform m_projectileSpawnPoint;
    public Transform projectileSpawnPoint { get { return m_projectileSpawnPoint; } }

    [SerializeField]
    private GameObject m_shotLandingParticles;
    public GameObject shotLandingParticles { get { return m_shotLandingParticles; } }
}

[CreateAssetMenu(fileName = "WeaponList", menuName = "ScriptableObject/WeaponList")]
public class WeaponInfo : ScriptableObject
{
    public List<WeaponItem> list;
}
