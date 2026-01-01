/*
 * PackManager页面的主逻辑
 */
using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlotPackManager : MonoBehaviour {
    public Image pageBackgroundImage;
    public GameObject plotItem;

    private void Awake() {
        Application.targetFrameRate = 120;
    }

    void Start() {
        if (ExternalResourceLoader.PageBackground == null) {
            ExternalResourceLoader.GetPageBackground();
        }
        pageBackgroundImage.sprite = ExternalResourceLoader.PageBackground;
        pageBackgroundImage.GetComponent<AspectRatioFitter>().aspectRatio =  ExternalResourceLoader.BackgroundAspectRatio;
        pageBackgroundImage.sprite = ExternalResourceLoader.PageBackground;
        pageBackgroundImage.GetComponent<AspectRatioFitter>().aspectRatio =
            ExternalResourceLoader.BackgroundAspectRatio;
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
                Sprite sprite = ExternalResourceLoader.LoadSpriteFormFile($"{packPath}/{image.ToString()}");
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

    public void OnExitButtonClick() {
        SceneManager.LoadSceneAsync("Homepage");
    }

    void RaiseOnSelectedPlotPack(string path) {
        GameEvents.GamingPackPath = path;
        SceneManager.LoadSceneAsync("Gaming");
    }
}
