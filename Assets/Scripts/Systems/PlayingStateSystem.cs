using Unity.Entities;

partial class PlayingStateSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .ForEach((Entity entity, ref GameStateComponent gameState, ref PlayingTimerComponent playingTimer) => {
                playingTimer.Timer += dt;
                if (playingTimer.Timer < playingTimer.Goal) {
                    return;
                }

                playingTimer.Timer = 0f;
                gameState.GameState = GameState.GameOver;
                ecb.SetComponentEnabled<PlayingTimerComponent>(entity, false);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class PlayingTimerUISystem : SystemBase {
    protected override void OnUpdate() {
        foreach (var (playingTimer, entity) in SystemAPI
                     .Query<PlayingTimerComponent>()
                     .WithEntityAccess()
                 ) {
            GamePlayingClockUI.Instance.SetTimer(playingTimer.Timer / playingTimer.Goal);
        }
    }
}