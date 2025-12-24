using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotPackManager : MonoBehaviour {
    public GameObject plotItem;
    
    void Start() {
        StartCoroutine(ListAllPackItem());
    }

    private void OnEnable() {
        GameEvents.OnSelectedPlotPack += RaiseOnSelectedPlotPack;
    }

    IEnumerator ListAllPackItem() {
        string[] packList = ExternalResourceLoader.ListAllPackDir();
        foreach (var packPath in packList) {
            JObject config = ExternalResourceLoader.GetPackInformation(packPath);
            if (config == null) continue;
            GameObject cloneObject = Instantiate(plotItem, gameObject.transform);
            cloneObject.SetActive(true);
            yield return null;
            PlotItem item = cloneObject.GetComponent<PlotItem>();
            JToken title = config["title"];
            JToken author = config["author"];
            JToken description = config["description"];
            JToken image = config["cover"];
            item.PointToPath = packPath;
            if (title != null) {
                item.title.text = title.ToString();
            } else {
                item.title.text = "Unknow";
            }

            yield return null;
            if (author != null && description != null) {
                item.SetDescription(description.ToString(), author.ToString());
            } else if (author == null && description == null) {
                item.SetDescription();
            } else if (author != null) {
                item.SetDescription("", author.ToString());
            } else {
               item.SetDescription(description.ToString()); 
            }

            yield return null;
            
            if (image != null) {
                Sprite sprite = ExternalResourceLoader.LoadSpriteFromFile($"{packPath}/{image.ToString()}");
                if (sprite != null) {
                    item.SetImage(sprite);
                }
            }
            
            item.Display();

            yield return null;
        }
    }

    private void OnDisable() {
        GameEvents.OnSelectedPlotPack -= RaiseOnSelectedPlotPack;
    }

    void RaiseOnSelectedPlotPack(string path) {
        GameEvents.GamingPackPath = path;
        SceneManager.LoadSceneAsync("Gaming");
        Debug.Log("Begin to Loading");
    }
    
    void Update() {
        
    }
}
