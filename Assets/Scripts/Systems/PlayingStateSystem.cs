using Unity.Collections;
using Unity.Entities;

partial class PlayingStateSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        NativeArray<GameStateComponent> gameStateNative = new NativeArray<GameStateComponent>(1, Allocator.TempJob);
        
        Entities
            .ForEach((Entity entity, ref GameStateComponent gameState, ref PlayingTimerComponent playingTimer) => {
                gameStateNative[0] = gameState;
                if (!gameState.IsGameActive()) {
                    return;
                }
                
                playingTimer.Timer += dt;
                if (playingTimer.Timer < playingTimer.Goal) {
                    return;
                }

                playingTimer.Timer = 0f;
                gameState.GameState = GameState.GameOver;
            }).Schedule();
        
        Entities
             .WithAll<CanGenerateOrdersComponent>()
             .WithNone<IsGenerateOrdersRestrictedComponent>()
             .ForEach((Entity entity) => {
                 if (!gameStateNative[0].IsGameActive()) {
                     ecb.SetComponentEnabled<IsGenerateOrdersRestrictedComponent>(entity, true);
                 }
             }).Schedule();
        
        Entities
            .WithAll<CanGenerateOrdersComponent, IsGenerateOrdersRestrictedComponent>()
            .ForEach((Entity entity) => {
                if (gameStateNative[0].IsGameActive()) {
                    ecb.SetComponentEnabled<IsGenerateOrdersRestrictedComponent>(entity, false);
                }
            }).Schedule();
         
         Entities
             .WithAll<CanBeSelectedComponent>()
             .WithNone<IsSelectionRestricted>()
             .ForEach((Entity entity) => {
                 if (!gameStateNative[0].IsGameActive()) {
                     ecb.SetComponentEnabled<IsSelectionRestricted>(entity, true);
                     ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, false);
                 }
             }).Schedule();
         
         Entities
             .WithDisposeOnCompletion(gameStateNative)
             .WithAll<CanBeSelectedComponent, IsSelectionRestricted>()
             .ForEach((Entity entity) => {
                 if (gameStateNative[0].IsGameActive()) {
                     ecb.SetComponentEnabled<IsSelectionRestricted>(entity, false);
                 }
             }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class PlayingTimerUISystem : SystemBase {
    protected override void OnUpdate() {
        foreach (var (playingTimer, gameState) in SystemAPI
                     .Query<PlayingTimerComponent, GameStateComponent>()
                 ) {
            if (gameState.IsGameActive()) {
                GamePlayingClockUI.Instance.SetTimer(playingTimer.Timer / playingTimer.Goal);
            }
        }
    }
}