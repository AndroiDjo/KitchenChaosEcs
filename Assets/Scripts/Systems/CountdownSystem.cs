using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial class CountdownSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;

        Entities
            .ForEach((Entity entity, ref GameStateComponent gameState, ref CountdownToStartComponent countdownToStart) => {
                if (gameState.GameState != GameState.CountdownToStart) {
                    return;
                }
                
                countdownToStart.Timer += dt;
                int currentNumber = (int)(countdownToStart.Goal - countdownToStart.Timer)+1;
                if (currentNumber != countdownToStart.NextDigit) {
                    countdownToStart.ShowNextDigit = true;
                }

                countdownToStart.NextDigit = currentNumber;
                if (countdownToStart.Timer < countdownToStart.Goal) {
                    return;
                }

                countdownToStart.Timer = 0f;
                gameState.GameState = GameState.GamePlaying;
            }).Schedule();
        
    }
}
partial class CountdownUISystem : SystemBase {
    protected override void OnUpdate() {
        
        foreach (var (countdownToStart, gameState, localTransform) in SystemAPI
                     .Query<RefRW<CountdownToStartComponent>, GameStateComponent, LocalTransform>()) {
            if (gameState.GameState != GameState.CountdownToStart) {
                GameCountdownUI.Instance.Stop();
            }
            else if (countdownToStart.ValueRO.ShowNextDigit) {
                GameCountdownUI.Instance.SetText(countdownToStart.ValueRO.NextDigit.ToString());
                SoundsManager.Instance.PlayWarningSound(localTransform.Position);
                countdownToStart.ValueRW.ShowNextDigit = false;
            }
        }
    }
}