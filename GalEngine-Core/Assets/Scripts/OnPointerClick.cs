using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerClick : MonoBehaviour, IPointerClickHandler {
    public string id;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        GameEvents.SendEventOnObjectClicked(id);
    }
}
