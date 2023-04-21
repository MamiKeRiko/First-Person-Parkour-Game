using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    private float musicVolume = 0.5f;
    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            musicVolume = value;
        }
    }
    public float MusicVolumeSave
    {
        get
        {
            return musicVolume;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            musicVolume = value;
            PlayerPrefs.SetFloat(AppPlayerPrefsKeys.MUSIC_VOLUME, musicVolume);
        }
    }

    private float sfxVolume = 0.5f;
    public float SfxVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            value = Mathf.Clamp(value, 0, 1);
            sfxVolume = value;
            PlayerPrefs.SetFloat(AppPlayerPrefsKeys.SFX_VOLUME, sfxVolume);
        }
    }

    public override void Awake()
    {
        base.Awake();

        musicVolume = PlayerPrefs.GetFloat(AppPlayerPrefsKeys.MUSIC_VOLUME, 0.5f);
        sfxVolume = PlayerPrefs.GetFloat(AppPlayerPrefsKeys.SFX_VOLUME, 0.5f);
    }


}
