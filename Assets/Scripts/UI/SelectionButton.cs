using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
public class SelectionButton : MonoBehaviour, IPointerClickHandler {
    [SerializeField] Image background;
    [SerializeField] TMP_Text label;

    public Action<SelectionButton> onClick;

    public void OnPointerClick(PointerEventData eventData) {
        onClick?.Invoke(this);
    }

    public void setText(string text) {
        label.text = text;
    }

    public void setSelected(bool selected, Color selectedColor, Color unselectedColor) {
        background.color = selected ? selectedColor : unselectedColor;
    }
}
}