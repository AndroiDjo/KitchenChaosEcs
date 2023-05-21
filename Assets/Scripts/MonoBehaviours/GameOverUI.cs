using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;
    public static GameOverUI Instance { get; private set; }

    private bool isActive;
    
    public void Awake() {
        Instance = this;
        gameObject.SetActive(false);
        isActive = false;
    }

    public void ShowScore(int score) {
        scoreText.SetText(score.ToString());
        if (!isActive) {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    public void Hide() {
        if (isActive) {
            gameObject.SetActive(false);
            isActive = false;
        }
    }
}
