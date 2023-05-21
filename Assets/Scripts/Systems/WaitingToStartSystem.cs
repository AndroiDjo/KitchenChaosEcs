using Unity.Entities;

partial class WaitingToStartSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        
        Entities
            .ForEach((Entity entity, ref GameStateComponent gameState, ref WaitingToStartComponent waitingToStart) => {
                if (gameState.GameState != GameState.WaitingToStart) {
                    return;
                }
                
                waitingToStart.Timer += dt;
                if (waitingToStart.Timer < waitingToStart.Goal) {
                    return;
                }

                waitingToStart.Timer = 0f;
                gameState.GameState = GameState.CountdownToStart;
            }).Schedule();
    }
}
