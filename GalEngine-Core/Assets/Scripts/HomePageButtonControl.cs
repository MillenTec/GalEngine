/*
 * 主页的主逻辑
 */
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePageButtonControl : MonoBehaviour {
    public Image pageBackgroundImage;
    public MessageBoxController messageBoxController;
    
    private void Awake() {
        Application.targetFrameRate = 120;
        Debug.Log(Application.companyName);
        Debug.Log(Application.productName);
    }

    private void Start() {
        if (ExternalResourceLoader.PageBackground == null) {
            ExternalResourceLoader.GetPageBackground();
        }
        pageBackgroundImage.sprite = ExternalResourceLoader.PageBackground;
        pageBackgroundImage.GetComponent<AspectRatioFitter>().aspectRatio =  ExternalResourceLoader.BackgroundAspectRatio;
        TextMeshProUGUI[] textMashProList = GetComponentsInChildren<TextMeshProUGUI>(); 
        FontResourceManager fontResourceManager = new FontResourceManager();
        fontResourceManager.SetTMProFont(textMashProList);
        StartCoroutine(WaitFontSetDone(fontResourceManager));
    }

    private void OnEnable() {
        messageBoxController.OnMessageBoxButtonClick += RaiseOnMessageBoxButtonClick;
    }

    private void OnDisable() {
        messageBoxController.OnMessageBoxButtonClick -= RaiseOnMessageBoxButtonClick;
    }

    IEnumerator WaitFontSetDone(FontResourceManager fontResourceManager) {
        while (!fontResourceManager.IsSetDone) {
            yield return null;
        }
    }

    public void StartButtonClick() {
        SceneManager.LoadSceneAsync("Gaming");
    }

    public void IsClickDevelopingWindow() {
        messageBoxController.gameObject.SetActive(true);
        messageBoxController.MessageBox("Coming Soon", "马上就会来到", MessageBoxType.OnlyOk);
    }

    void RaiseOnMessageBoxButtonClick(Button button) {
        Debug.Log(button);
        if (button == Button.Confirm) {
            messageBoxController.CloseMessageBox();
        }
    }

    public void ClickSettingsButton() {
        //StartCoroutine(Animation.CanvasFadeIn(settingsWindow, 0.1f));
    }

    public void ClickStartButton() {
        SceneManager.LoadSceneAsync("PackManager");
    }

    public void ClickCloseButton() {
        Application.Quit();
    }
}