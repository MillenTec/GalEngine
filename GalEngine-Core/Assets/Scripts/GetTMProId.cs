/*
 * 这是一个工具脚本，建议在编译游戏时把其他脚本中关于此脚本的引用都删除干净
 */

using Newtonsoft.Json.Linq;
using UnityEngine;
using TMPro;
using System.Collections;
using System.IO;

public class GetTMProId {
    private string _jsonData;
    private JObject _idList = new JObject();

    public void GetAllTMProId(TextMeshProUGUI[] tmProList) {
        CoroutineProxy.Instance.StartCoroutine(TraverseTMPro(tmProList));
    }

    IEnumerator TraverseTMPro(TextMeshProUGUI[] tmProList) {
        foreach (var tmPro in tmProList) {
            TextIdMarker idMarker = tmPro.GetComponent<TextIdMarker>();
            string id = idMarker.textID;
            string value = tmPro.text;
            _idList[id] = value;
            yield return null;
        }

        _jsonData = _idList.ToString();
        File.WriteAllText(Application.persistentDataPath + "/tempLocal.json", _jsonData);
    }
}