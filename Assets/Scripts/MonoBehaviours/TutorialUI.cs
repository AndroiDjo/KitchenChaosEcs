using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    
    private CustomInputSystem customInputSystem;

    public static TutorialUI Instance { get; private set; } 
    private void Awake() {
        Instance = this;
        customInputSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CustomInputSystem>();
    }

    private void Start() {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
        UpdateBindingVisible();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
    
    private void UpdateBindingVisible() {
        moveUpText.SetText(customInputSystem.GetBindingKey(CustomInputSystem.Binding.MoveUp));
        moveDownText.SetText(customInputSystem.GetBindingKey(CustomInputSystem.Binding.MoveDown));
        moveLeftText.SetText(customInputSystem.GetBindingKey(CustomInputSystem.Binding.MoveLeft));
        moveRightText.SetText(customInputSystem.GetBindingKey(CustomInputSystem.Binding.MoveRight));
        interactText.SetText(customInputSystem.GetBindingKey(CustomInputSystem.Binding.Interact));
        interactAlternateText.SetText(customInputSystem.GetBindingKey(CustomInputSystem.Binding.InteractAlternate));
        pauseText.SetText(customInputSystem.GetBindingKey(CustomInputSystem.Binding.Pause));
    }
}
