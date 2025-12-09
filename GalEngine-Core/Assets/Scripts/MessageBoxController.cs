using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum MessageBoxType { Normal, OnlyCancel, OnlyOk }
public enum Button {Confirm, Cancel}

public class MessageBoxController : MonoBehaviour {
    private CanvasGroup _canvasGroup;
    public CanvasGroup okButton;
    public CanvasGroup cancelButton;
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI messageTMP;
    
    public event Action<Button> OnMessageBoxButtonClick;
    
    private void Awake() {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        _canvasGroup.alpha = 0;
    }
    
    public void MessageBox(string title, string message, MessageBoxType type) {
        titleTMP.text = title;
        messageTMP.text = message;
        if (type == MessageBoxType.Normal) {
            okButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        }else if (type == MessageBoxType.OnlyCancel) {
            okButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(true);
        }else if (type == MessageBoxType.OnlyOk) {
            okButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(false);
        }
        StartCoroutine(Animation.CanvasFadeIn(_canvasGroup, 0.1f));
    }

    public void CloseMessageBox() {
        StartCoroutine(Animation.CanvasFadeOut(_canvasGroup, 0.1f));
    }

    public void ClickCancelButton() {
        OnMessageBoxButtonClick?.Invoke(Button.Cancel);
    }

    public void ClickOkButton() {
        OnMessageBoxButtonClick?.Invoke(Button.Confirm);
    }
}
