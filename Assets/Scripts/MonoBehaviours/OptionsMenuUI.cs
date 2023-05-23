using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour {
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI soundEffectsButtonText;
    [SerializeField] private TextMeshProUGUI musicVolumeButtonText;
    [SerializeField] private float volumeStep = 0.1f;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private GameObject rebindKeyUI;

    private CustomInputSystem customInputSystem;
    public static OptionsMenuUI Instance { get; private set; }
    
    private void Awake() {
        Instance = this;
        customInputSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<CustomInputSystem>();
        soundEffectsButton.onClick.AddListener(OnClickSoundEffects);
        musicVolumeButton.onClick.AddListener(OnClickMusicVolume);
        backButton.onClick.AddListener(() => {
            Hide();
        });
        
        moveUpButton.onClick.AddListener(() => { RebindKey(CustomInputSystem.Binding.MoveUp); });
        moveDownButton.onClick.AddListener(() => { RebindKey(CustomInputSystem.Binding.MoveDown); });
        moveLeftButton.onClick.AddListener(() => { RebindKey(CustomInputSystem.Binding.MoveLeft); });
        moveRightButton.onClick.AddListener(() => { RebindKey(CustomInputSystem.Binding.MoveRight); });
        interactButton.onClick.AddListener(() => { RebindKey(CustomInputSystem.Binding.Interact); });
        interactAlternateButton.onClick.AddListener(() => { RebindKey(CustomInputSystem.Binding.InteractAlternate); });
        pauseButton.onClick.AddListener(() => { RebindKey(CustomInputSystem.Binding.Pause); });
    }

    private void OnClickMusicVolume() {
        float currentVolume = MusicManager.Instance.GetVolume();
        currentVolume += volumeStep;
        if (currentVolume > 1f) {
            currentVolume = 0f;
        }
        SetMusicVolume(currentVolume);
    }

    private void Start() {
        UpdateBindingVisible();
        UpdateMusicVisible();
        UpdateSoundEffectsVisible();
        HideRebindingKeyUI();
        Hide();
    }

    private void OnClickSoundEffects() {
        float currentVolume = SoundsManager.Instance.GetVolume();
        currentVolume += volumeStep;
        if (currentVolume > 1f) {
            currentVolume = 0f;
        }
        SetSoundEffectsVolume(currentVolume);
    }

    private void UpdateMusicVisible() {
        float volume = MusicManager.Instance.GetVolume();
        musicVolumeButtonText.SetText("MUSIC VOLUME: "+math.round(volume*10).ToString());
    }
    private void SetMusicVolume(float volume) {
        MusicManager.Instance.SetVolume(volume);
        UpdateMusicVisible();
    }

    private void UpdateSoundEffectsVisible() {
        float volume = SoundsManager.Instance.GetVolume();
        soundEffectsButtonText.SetText("SOUND EFFECTS: "+math.round(volume*10).ToString());
    }
    private void SetSoundEffectsVolume(float volume) {
        SoundsManager.Instance.SetVolume(volume);
        UpdateSoundEffectsVisible();
    }

    public void Show() {
        gameObject.SetActive(true);
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

    private void RebindKey(CustomInputSystem.Binding binding) {
        ShowRebindingKeyUI();
        
        customInputSystem.RebindBinding(binding, () => {
            UpdateBindingVisible();
            HideRebindingKeyUI();
        });
    }

    private void ShowRebindingKeyUI() {
        rebindKeyUI.SetActive(true);
    }

    private void HideRebindingKeyUI() {
        rebindKeyUI.SetActive(false);
    }
}
