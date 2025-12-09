using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System;

public class HomePageBtnControl : MonoBehaviour {
    public TextMeshProUGUI testText;

    public CanvasGroup messageBox;
    private MessageBoxController _messageBoxController;
    
    private void Awake() {
        Application.targetFrameRate = 120;
        _messageBoxController = messageBox.GetComponent<MessageBoxController>();
        if (_messageBoxController == null) {
            _messageBoxController = messageBox.AddComponent<MessageBoxController>();
        }
    }

    private void Start() {
        TextMeshProUGUI[] textMashProList = GetComponentsInChildren<TextMeshProUGUI>(); 
        FontResourceManager fontResourceManager = new FontResourceManager();
        fontResourceManager.SetTMProFont(textMashProList);
        StartCoroutine(WaitFontSetDone(fontResourceManager));
    }

    private void OnEnable() {
        _messageBoxController.OnMessageBoxButtonClick += RaiseOnMessageBoxButtonClick;
    }

    private void OnDisable() {
        _messageBoxController.OnMessageBoxButtonClick -= RaiseOnMessageBoxButtonClick;
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

    public void IsClickDevelopingWindow() {
        _messageBoxController.MessageBox("Coming Soon", "马上就会来到", MessageBoxType.OnlyOk);
    }

    void RaiseOnMessageBoxButtonClick(Button button) {
        Debug.Log(button);
        if (button == Button.Confirm) {
            _messageBoxController.CloseMessageBox();
        }
    }

    public void ClickCloseButton() {
        Application.Quit();
    }
}