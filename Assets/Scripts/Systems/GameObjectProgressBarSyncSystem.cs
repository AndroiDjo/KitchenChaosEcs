using Unity.Entities;
using UnityEngine;

partial struct GameObjectProgressBarSyncSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        foreach (var (goProgressBar, progressBarValue) in SystemAPI
                     .Query<GameObjectProgressBarComponent, ProgressBarValueComponent>()) {
            float value = progressBarValue.Value;
            if (value < 0) {
                value = 0f;
            } else if (value > 1) {
                value = 1f;
            }

            goProgressBar.Image.fillAmount = value;
            if (Mathf.Approximately(value, 0f) || Mathf.Approximately(value, 1f)) {
                if (goProgressBar.ProgressBarGO.activeSelf)
                    goProgressBar.ProgressBarGO.SetActive(false);
            }
            else {
                if (!goProgressBar.ProgressBarGO.activeSelf)
                    goProgressBar.ProgressBarGO.SetActive(true);
            }
        }
    }
}