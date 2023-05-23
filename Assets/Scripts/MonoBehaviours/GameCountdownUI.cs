using System;
using TMPro;
using UnityEngine;

public class GameCountdownUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;

    private const string ANIMATION_NUMBER_POPUP = "NumberPopup";
    public static GameCountdownUI Instance { get; private set; }

    private bool isActive;

    public void Awake() {
        Instance = this;
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    public void SetText(string currentCountdown) {
        countdownText.SetText(currentCountdown);
        if (!isActive) {
            gameObject.SetActive(true);
            isActive = true;
        }
        animator.SetTrigger(ANIMATION_NUMBER_POPUP);
    }

    public void Stop() {
        if (isActive) {
            isActive = false;
            gameObject.SetActive(false);
        }
    }
    
}