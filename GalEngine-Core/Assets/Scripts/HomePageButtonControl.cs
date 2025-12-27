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
    }

    private void Start() {
        if (Directory.Exists($"{PathManager.GetApplicationRootPath()}/UI/pageBackground")) {
            string uiPath = $"{PathManager.GetApplicationRootPath()}/UI/pageBackground";
            if (File.Exists($"{uiPath}/config.json")) {
                string data = File.ReadAllText($"{uiPath}/config.json");
                JObject config = JObject.Parse(data);
                if (config["image"] != null) {
                    ExternalResourceLoader.PageBackground =
                        ExternalResourceLoader.LoadSpriteFromFile($"{uiPath}/{config["image"]}");
                    pageBackgroundImage.sprite = ExternalResourceLoader.PageBackground;
                    /*
                    if (config["aspectRatio"] != null) {
                        ExternalResourceLoader.BackgroundAspectRatio = config["aspectRatio"].ToObject<float>();
                        pageBackgroundImage.GetComponent<AspectRatioFitter>().aspectRatio =  ExternalResourceLoader.BackgroundAspectRatio;
                    }
                    */
                    float width = ExternalResourceLoader.PageBackground.rect.width;
                    float height = ExternalResourceLoader.PageBackground.rect.height;
                    ExternalResourceLoader.BackgroundAspectRatio = width / height;
                    pageBackgroundImage.GetComponent<AspectRatioFitter>().aspectRatio =  ExternalResourceLoader.BackgroundAspectRatio;
                }
            }
        }
        TextMeshProUGUI[] textMashProList = GetComponentsInChildren<TextMeshProUGUI>(); 
        FontResourceManager fontResourceManager = new FontResourceManager();
        fontResourceManager.SetTMProFont(textMashProList);
        StartCoroutine(WaitFontSetDone(fontResourceManager));
        Debug.Log(PathManager.GetPacksRootPath());
        Debug.Log(ExternalResourceLoader.ListAllPackDir()[1]);
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