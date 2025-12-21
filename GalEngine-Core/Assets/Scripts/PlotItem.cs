using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlotItem : MonoBehaviour {
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image image;
    public string PointToPath;

    public void SetTitle(string value) {
        title.text = value;
    }

    public void SetDescription(string descriptionValue = "", string author = "Anonymous") {
        string value = $"Author: {author}\n{descriptionValue}";
        description.text = value;
    }

    public void OnButtonClick() {
        GameEvents.SendEventOnSelectedPlotPack(PointToPath);
    }

    public void SetImage(Sprite sprite) {
        image.color = new Color(256, 256, 256);
        image.sprite = sprite;
    }
}
