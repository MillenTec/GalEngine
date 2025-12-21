using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlotItem : MonoBehaviour {
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image image;
    public string PointToPath;

    private CanvasGroup _canvasGroup;

    public void Display() {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) {
            gameObject.AddComponent<CanvasGroup>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        _canvasGroup.alpha = 0;
        StartCoroutine(Animation.CanvasFadeIn(_canvasGroup, 0.2f));
    }

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
