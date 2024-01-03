using System;
using UnityEngine;
using Utils;
using Utils.Interfaces;

namespace Windows {
public class Window : MonoBehaviour, AlphaAdjustable {
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected float fadeDuration = 0.2f;

    public Action onHideAction;
    
    public float alpha {
        get => canvasGroup.alpha;
        set => canvasGroup.alpha = value;
    }

    protected void show(float duration, Action action = null) {
        gameObject.SetActive(true);
        canvasGroup.interactable = false;
        alpha = 0f;
        StartCoroutine(Coroutines.fadeTo(this, 1f, duration, () => {
            canvasGroup.interactable = true;
            action?.Invoke();
        }));
    }

    protected void hide(float duration, Action action = null) {
        canvasGroup.interactable = false;
        alpha = 1f;
        StartCoroutine(Coroutines.fadeTo(this, 0f, duration, () => {
            canvasGroup.interactable = true;
            gameObject.SetActive(false);
            onHideAction?.Invoke();
            onHideAction = null;
            action?.Invoke();
        }));
    }
    
    public void show(Action action = null) {
        show(fadeDuration, action);
    }

    public void hide(Action action = null) {
        hide(fadeDuration, action);
    }

    public void setVisible(bool visible) {
        gameObject.SetActive(visible);
    }
}
}