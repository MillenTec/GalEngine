using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindowController : MonoBehaviour {
    private enum SettingsPage {Common, Audio}
    private event Action<SettingsPage> OnSettingsPageChange;
    private SettingsPage _settingsPageType = SettingsPage.Common;
    private SettingsPage SettingsPageType {
        get => _settingsPageType;
        set {
            if (value != _settingsPageType) {
                _settingsPageType = value;
                OnSettingsPageChange?.Invoke(_settingsPageType);          
            }
        }
    }
    private List<TMP_Dropdown.OptionData> _languageDropdownOption = new List<TMP_Dropdown.OptionData>() {
        new("简体中文"),
        new("繁體中文"),
        new("English"),
        new("日本語")
    };

    public CanvasGroup commonSettings;
    public CanvasGroup audioSettings;
    public TMP_Dropdown languageDropdown;

    #region Dock
    public Image commonSettingsImage;
    public Image audioSettingsImage;
    #endregion
    
    void Start() {
        languageDropdown.options = _languageDropdownOption;
    }

    private void OnEnable() {
        OnSettingsPageChange += RaiseOnSettingsPageChange;
    }

    void Update() {
        
    }

    void RaiseOnSettingsPageChange(SettingsPage settingsPage) {
        if (settingsPage == SettingsPage.Common) {
            commonSettingsImage.color = new Color(228, 254, 228);
            audioSettingsImage.color = new Color(112, 228, 254);
            if (audioSettings.alpha != 0) StartCoroutine(Animation.CanvasFadeOut(audioSettings, 0.1f));
            StartCoroutine(Animation.CanvasFadeIn(commonSettings, 0.1f));
        }else if (settingsPage == SettingsPage.Audio) {
            audioSettingsImage.color = new Color(228, 254, 228);
            commonSettingsImage.color = new Color(112, 228, 254);
            if (audioSettings.alpha != 0) StartCoroutine(Animation.CanvasFadeOut(commonSettings, 0.1f));
            StartCoroutine(Animation.CanvasFadeIn(audioSettings, 0.1f));
        }
    }

    public void ClickCommonSettingsPageButton() {
        SettingsPageType = SettingsPage.Common;
    }

    public void ClickAudioSettingsPageButton() {
        SettingsPageType = SettingsPage.Audio;
    }
}
