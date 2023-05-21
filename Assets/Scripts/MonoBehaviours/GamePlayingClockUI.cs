using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour {
    [SerializeField] private Image clockImage;
    
    public static GamePlayingClockUI Instance { get; private set; }
    private bool isActive;

    public void Awake() {
        Instance = this;
        gameObject.SetActive(false);
        isActive = false;
    }

    public void SetTimer(float currentTime) {
        clockImage.fillAmount = currentTime;
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
