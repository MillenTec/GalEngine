using System;
using System.Collections;
using UnityEngine;
using TMPro;

public enum MessageBoxType { Normal, OnlyCancel, OnlyOk }

public class MessageBoxController : MonoBehaviour {
    private CanvasGroup _canvasGroup;
    public CanvasGroup okButton;
    public CanvasGroup cancelButton;
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI messageTMP;

    public bool isCancelButtonClick = false;
    public bool isOkButtonClick = false;
    private void Awake() {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _canvasGroup.alpha = 0;
        okButton.alpha = 0;
        cancelButton.alpha = 0;
    }
    
    public void MessageBox(string title, string message, MessageBoxType type) {
        isCancelButtonClick = false;
        isOkButtonClick = false;
        titleTMP.text = title;
        messageTMP.text = message;
        if (type == MessageBoxType.Normal) {
            okButton.alpha = 1;
            cancelButton.alpha = 1;
        }else if (type == MessageBoxType.OnlyCancel) {
            okButton.alpha = 0;
            cancelButton.alpha = 1;
        }else if (type == MessageBoxType.OnlyOk) {
            okButton.alpha = 1;
            cancelButton.alpha = 0;
        }
        StartCoroutine(Animation.CanvasFadeIn(_canvasGroup, 0.1f));
    }

    public void CloseMessageBox() {
        StartCoroutine(Animation.CanvasFadeOut(_canvasGroup, 0.1f));
    }

    public void ClickCancelButton() {
        isCancelButtonClick = true;
    }

    public void ClickOkButton() {
        isOkButtonClick = true;
    }
}
