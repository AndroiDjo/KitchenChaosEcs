using System;
using Unity.Entities;

partial class WaitingToStartSystem : SystemBase {

    private CustomInputSystem _customInputSystem;

    protected override void OnCreate() {
        _customInputSystem = this.World.GetOrCreateSystemManaged<CustomInputSystem>();
        _customInputSystem.OnInteractAction += CustomInputSystemOnOnInteractAction;
    }

    private void CustomInputSystemOnOnInteractAction(object sender, EventArgs e) {
        foreach (var (gameState, entity) in SystemAPI
                     .Query<RefRW<GameStateComponent>>()
                     .WithEntityAccess()
                     .WithAll<WaitingToStartComponent>()
                 ) {
            if (gameState.ValueRO.GameState != GameState.WaitingToStart) {
                return;
            }

            gameState.ValueRW.GameState = GameState.CountdownToStart;
            TutorialUI.Instance.Hide();
        }
    }

    protected override void OnUpdate() {
    }
}
