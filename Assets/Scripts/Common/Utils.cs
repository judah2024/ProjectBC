using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    public static IEnumerator CoFade(CanvasGroup ui, float to, float from, float duration)
    {
        ui.gameObject.SetActive(true);
        float t = 0f;

        ui.alpha = to;
        while (t < duration)
        {
            float r = t / duration;

            ui.alpha = Mathf.Lerp(to, from, r);

            yield return null;
            t += Time.deltaTime;
        }
        ui.alpha = from;
        ui.gameObject.SetActive(false);
    }
    
    public static IEnumerator CoFadeIn(CanvasGroup ui, float duration)
    {
        yield return CoFade(ui, 0, 1, duration);
    }
    
    public static IEnumerator CoFadeOut(CanvasGroup ui, float duration)
    {
        yield return CoFade(ui, 1, 0, duration);
    }
    
    public static IEnumerator CoFade(Image image, float to, float from, float duration)
    {
        image.gameObject.SetActive(true);
        
        float t = 0f;
        Color c = image.color;
        
        c.a = to;
        image.color = c;
        while (t < duration)
        {
            float r = t / duration;

            c.a = Mathf.Lerp(to, from, r);
            image.color = c;

            yield return null;
            t += Time.deltaTime;
        }
        c.a = from;
        image.color = c;
        
        image.gameObject.SetActive(false);
    }
    public static IEnumerator CoFadeIn(Image image, float duration)
    {
        yield return CoFade(image, 0, 1, duration);
    }
    
    public static IEnumerator CoFadeOut(Image image, float duration)
    {
        yield return CoFade(image, 1, 0, duration);
    }

}
