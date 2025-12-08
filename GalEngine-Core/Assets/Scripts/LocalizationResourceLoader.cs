using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Collections;

class LocalizationResourceLoader {
    private JObject _langJsonData;
    
    public LocalizationResourceLoader() {
        
    }

    public void GetLanguageResource(TextAsset lang) {
        string langText = lang.text;
        _langJsonData = JObject.Parse(langText);
    }

    public void SetLanguage(TextMeshProUGUI[] textList) {
        CoroutineProxy.Instance.StartCoroutine(TraverseText(textList));
    }

    IEnumerator TraverseText(TextMeshProUGUI[] textList) {
        foreach (var textMeshPro in textList) {
            TextIdMarker idMarker = textMeshPro.GetComponent<TextIdMarker>();
            string id = idMarker.textID;
            if (idMarker == null || id == null) continue;
            textMeshPro.text = (string)_langJsonData[id];
            yield return null;
        }
    }
}