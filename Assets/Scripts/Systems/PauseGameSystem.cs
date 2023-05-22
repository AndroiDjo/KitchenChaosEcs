using System;
using Unity.Entities;
using UnityEngine;

partial class PauseGameSystem : SystemBase {
    
    protected override void OnStartRunning() {
        var customInputSystem = this.World.GetExistingSystemManaged<CustomInputSystem>();
        customInputSystem.OnPauseAction += CustomInputSystemOnPauseAction;
    }

    private void CustomInputSystemOnPauseAction(object sender, EventArgs e) {
        TogglePause();
    }
    
    private bool isGamePaused;
    public void TogglePause() {
        if (isGamePaused) {
            UnityEngine.Time.timeScale = 1f;
            isGamePaused = false;
            PauseMenuUI.Instance.Hide();
        }
        else {
            UnityEngine.Time.timeScale = 0f;
            isGamePaused = true;
            PauseMenuUI.Instance.Show();
        }
    }

    protected override void OnUpdate() {
        
    }
}