using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class HomePageButtonControl : MonoBehaviour {
    
    public MessageBoxController messageBoxController;
    
    private void Awake() {
        Application.targetFrameRate = 120;
    }

    private void Start() {
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

    private void Update() {

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

    public void ClickCloseButton() {
        Application.Quit();
    }
}