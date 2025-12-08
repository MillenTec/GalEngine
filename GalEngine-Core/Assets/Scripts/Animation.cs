using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class Animation {
    public static IEnumerator CanvasFadeIn(CanvasGroup canvas, float speed) {
        canvas.blocksRaycasts = true;
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
    }
}