using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public float fadeDuration = 1f;
    
    private Image FadeImage => Managers.UI.loadingImage;

    private void Awake()
    {
        if (Managers.Scene != this)
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        Managers.UI.Clear();
        SceneManager.LoadScene(sceneName);
        StartCoroutine(Utils.CoFadeOut(FadeImage, fadeDuration));
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(CoLoadSceneAsync(sceneName));
    }

    private IEnumerator CoLoadSceneAsync(string sceneName)
    {
        yield return Utils.CoFadeIn(FadeImage, fadeDuration);
        
        // 씬이 전환되면 UI스택을 초기화한다.
        Managers.UI.Clear();

        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float progress = 0f;
        while (op.isDone == false)
        {
            progress = Mathf.Clamp01(op.progress / 0.9f);

            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
        
        yield return Utils.CoFadeOut(FadeImage, fadeDuration);
    }
    
}
