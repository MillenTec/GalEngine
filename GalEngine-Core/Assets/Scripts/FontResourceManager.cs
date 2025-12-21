using System.Collections;
using UnityEngine;
using TMPro;

public class CoroutineProxy : MonoBehaviour {
    private static CoroutineProxy _instance;

    public static CoroutineProxy Instance {
        get {
            if (_instance == null) {
                GameObject startCoroutine = new GameObject("CoroutineProxy");
                DontDestroyOnLoad(startCoroutine);
                _instance = startCoroutine.AddComponent<CoroutineProxy>();
            }

            return _instance;
        }
    }
}

public class FontResourceManager {
    private TMP_FontAsset _fullFontBold;
    private TMP_FontAsset _asciiFontBold;
    private TMP_FontAsset _fullFontLight;
    private TMP_FontAsset _asciiFontLight;
    private TMP_FontAsset _fullFontMedium;
    private TMP_FontAsset _asciiFontMedium;
    public bool IsSetDone;

    public FontResourceManager() {
        _fullFontBold = Resources.Load<TMP_FontAsset>("Fonts/fullBold");
        _asciiFontBold = Resources.Load<TMP_FontAsset>("Fonts/asciiBold");
        _fullFontLight = Resources.Load<TMP_FontAsset>("Fonts/fullLight");
        _asciiFontLight = Resources.Load<TMP_FontAsset>("Fonts/asciiLight");
        _fullFontMedium = Resources.Load<TMP_FontAsset>("Fonts/fullMedium");
        _asciiFontMedium = Resources.Load<TMP_FontAsset>("Fonts/asciiMedium");
        IsSetDone = false;
        Debug.Log($"{_asciiFontLight} {_fullFontLight}");
    }

    public void SetTMProFont(TextMeshProUGUI[] textMeshProList) {
        CoroutineProxy.Instance.StartCoroutine(GetFontWeight(textMeshProList));
    }

    IEnumerator GetFontWeight(TextMeshProUGUI[] textMeshProList) {
        if (textMeshProList == null) yield break;
        foreach (var TMProUGUI in textMeshProList) {
            if (TMProUGUI == null) continue;

            FontWeightMarker weightMarker = TMProUGUI.GetComponent<FontWeightMarker>();
            if (weightMarker == null) continue;
            FontWeightType fontWeightMark = weightMarker.weight;
            
            if (fontWeightMark == FontWeightType.Bold) {
                SetFont(TMProUGUI, _fullFontBold, _asciiFontBold);
                continue;
            }

            if (fontWeightMark == FontWeightType.Light) {
                SetFont(TMProUGUI, _fullFontLight, _asciiFontLight);
                continue;
            }

            if (fontWeightMark == FontWeightType.Medium) {
                SetFont(TMProUGUI, _fullFontMedium, _asciiFontMedium);
                continue;
            }

            yield return null;
        }

        IsSetDone = true;
    }

    private bool SetFont(TextMeshProUGUI textMeshPro, TMP_FontAsset fullAsset, TMP_FontAsset asciiAsset) {
        if (fullAsset != null) {
            textMeshPro.font = fullAsset;
        }else if (asciiAsset != null) {
            textMeshPro.font = asciiAsset;
        } else {
            Debug.Log("ERROR: 没有找到任何字体");
        }

        return true;
    }
}