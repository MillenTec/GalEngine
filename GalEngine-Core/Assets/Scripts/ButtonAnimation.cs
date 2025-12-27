using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    public TextMeshProUGUI text;
    public Color color = Color.black;
    public Color hoverColor =  Color.black;
    
    // 用IPointerEnterHandler/ExitHandler自动触发事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        
        StartCoroutine(Animation.ChangeColor(iconImage, iconImage.color, hoverColor, 0.1f));
        StartCoroutine(Animation.ChangeColor(text, text.color, hoverColor, 0.1f));
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        
        StartCoroutine(Animation.ChangeColor(iconImage, iconImage.color, color, 0.1f));
        StartCoroutine(Animation.ChangeColor(text, text.color, color, 0.1f));
    }
}
