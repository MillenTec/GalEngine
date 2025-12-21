using UnityEngine;

public class NameTitleController : MonoBehaviour {
    public GameObject nameTitle;
    public GameObject descriptionText;

    private RectTransform _nameRectTransform;
    private RectTransform _descriptionRectTransform;

    void Start() {
        _nameRectTransform = nameTitle.GetComponent<RectTransform>();
        if (_nameRectTransform == null) {
            _nameRectTransform = nameTitle.AddComponent<RectTransform>();
        }
        _descriptionRectTransform = descriptionText.GetComponent<RectTransform>();
        if (_descriptionRectTransform == null) {
            _descriptionRectTransform = descriptionText.AddComponent<RectTransform>();
        }
    }
    
    void Update() {
        Vector3 nameTitlePosition = _nameRectTransform.position;
        float nameTitleWidth = _nameRectTransform.rect.width;
        nameTitlePosition.x += 25 + nameTitleWidth/2;
        _descriptionRectTransform.position = nameTitlePosition;
    }
}
