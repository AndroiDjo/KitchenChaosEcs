using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryPopupUI : MonoBehaviour {
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI deliveryText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color successColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Color failureColor;
    [SerializeField] private Sprite failureSprite;
    [SerializeField] private Animator animator;

    private const string ANIMATION_POPUP = "Popup";
    private bool isActive = true;
    
    private void Awake() {
        Hide();
    }

    public void DeliverySuccess() {
        Show();

        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        deliveryText.SetText("DELIVERY SUCCESS");
        animator.SetTrigger(ANIMATION_POPUP);
    }
    
    public void DeliveryFailed() {
        Show();

        backgroundImage.color = failureColor;
        iconImage.sprite = failureSprite;
        deliveryText.SetText("DELIVERY FAILED");
        animator.SetTrigger(ANIMATION_POPUP);
    }

    private void Show() {
        if (!isActive) {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    private void Hide() {
        if (isActive) {
            gameObject.SetActive(false);
            isActive = false;
        }
    }
}
