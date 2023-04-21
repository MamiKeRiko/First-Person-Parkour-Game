using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppScenes
{
    public static readonly int MAIN_MENU = 0;
    public static readonly int LOADING_SCREEN = 1;
    public static readonly int GAME_SCENE = 2;
}

public class AppPlayerPrefsKeys
{
    public static readonly string MUSIC_VOLUME = "MusicVolume";
    public static readonly string SFX_VOLUME = "SFXVolume";
}

public class AppPaths
{
    public static readonly string PERSISTENT_DATA = Application.persistentDataPath;
    public static readonly string RESOURCE_SFX = "Sounds/SoundEffects";
    public static readonly string RESOURCE_MUSIC = "Sounds/Music";
}

public class AppSounds
{
    
}
