using System;
using TMPro;
using UnityEngine;

public class GameCountdownUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI countdownText;
    public static GameCountdownUI Instance { get; private set; }

    private bool isActive;

    public void Awake() {
        Instance = this;
    }

    public void SetText(string currentCountdown) {
        countdownText.SetText(currentCountdown);
        if (!isActive) {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    public void Stop() {
        if (isActive) {
            isActive = false;
            gameObject.SetActive(false);
        }
    }
    
}