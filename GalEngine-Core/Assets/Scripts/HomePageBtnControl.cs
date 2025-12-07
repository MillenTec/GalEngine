using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HomePageBtnControl : MonoBehaviour {
    public CanvasGroup developingWindow;
    public TextMeshProUGUI testText;
    
    private void Awake() {
        Application.targetFrameRate = 120;
    }

    private void Start() {
        TextMeshProUGUI[] textMashProList = GetComponentsInChildren<TextMeshProUGUI>(); 
        FontResourceManager fontResourceManager = new FontResourceManager();
        fontResourceManager.SetTMProFont(textMashProList);
        StartCoroutine(WaitFontSetDone(fontResourceManager));
    }

    private void Update() {

    }

    IEnumerator WaitFontSetDone(FontResourceManager fontResourceManager) {
        while (!fontResourceManager.IsSetDone) {
            yield return null;
        }
    }

    public void StartButtonClick() {
        SceneManager.LoadScene("Gaming");
    }

    private IEnumerator CanvasFadeIn(CanvasGroup canvas, float speed) {
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

    private IEnumerator CanvasFadeOut(CanvasGroup canvas, float speed) {
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


    public void IsClickDevelopingWindow() {
        StartCoroutine(CanvasFadeIn(developingWindow, 0.1f));
    }

    public void IsCloseDevelopingWindow() {
        StartCoroutine(CanvasFadeOut(developingWindow, 0.1f));
    }

    public void ClickCloseButton() {
        Application.Quit();
    }
}