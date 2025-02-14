using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITitle : UIScene
{
    [Range(0, 1)] public float minA = 0.5f;
    public float fadeDuration = 1f;
    public CanvasGroup TouchToStartGroup;

    public override void OnOpen()
    {
        base.OnOpen();

        StartCoroutine(CoFadeStartText());
    }


    private IEnumerator CoFadeStartText()
    {
        float maxA = 1f;
        while (true)
        {
            yield return Utils.CoFade(TouchToStartGroup, minA, maxA, fadeDuration);
            yield return Utils.CoFade(TouchToStartGroup, maxA, minA, fadeDuration);
        }
    }

    public void OnTitleScreenClick()
    {
        // Main 씬 로드
        LoggerEx.Log("메인 씬 로드!");
        Managers.Scene.LoadSceneAsync("Main");
    }
}
