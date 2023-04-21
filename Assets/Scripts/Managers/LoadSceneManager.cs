using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    AsyncOperation operation;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FinishLoading;
        StartCoroutine("ProgressBar");
    }

    IEnumerator ProgressBar()
    {
        yield return null;
    }

    void FinishLoading(Scene scene, LoadSceneMode mode)
    {

    }
}
