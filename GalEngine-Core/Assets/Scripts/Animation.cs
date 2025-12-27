using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public static class Animation {
    public static IEnumerator CanvasFadeIn(CanvasGroup canvas, float speed) {
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
        canvas.gameObject.SetActive(true);
        float time = 0f;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (time <= speed) {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, time / speed);
            canvas.alpha = currentAlpha;
            yield return null;
        }

        canvas.alpha = 1f;
    }

    public static IEnumerator CanvasFadeOut(CanvasGroup canvas, float speed) {
        float time = 0f;
        float startAlpha = 1f;
        float endAlpha = 0f;

        while (time <= speed) {
            time += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, time / speed);
            canvas.alpha = currentAlpha;
            yield return null;
        }

        canvas.alpha = 0f;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        canvas.gameObject.SetActive(false);
    }
    
    public static IEnumerator ChangeColor<T>(T target, Color start, Color end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            if (target is Image image)
                image.color = Color.Lerp(start, end, t);
            else if (target is TextMeshProUGUI text)
                text.color = Color.Lerp(start, end, t);
            
            yield return null;
        }
        
        // 确保最终颜色（避免浮点误差）
        if (target is Image image1) image1.color = end;
        else if (target is TextMeshProUGUI text) text.color = end;
    }
}