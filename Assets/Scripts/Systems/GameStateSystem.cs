using Unity.Collections;
using Unity.Entities;

partial class GameStateSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        NativeArray<GameStateComponent> gameStateNative = new NativeArray<GameStateComponent>(1, Allocator.TempJob);

        Entities
            .ForEach((in GameStateComponent gameState) => {
                gameStateNative[0] = gameState;
            }).Schedule();
        
        Entities
            .WithAll<CanWaitingToStartComponent, IsStateChangedComponent>()
            .WithNone<WaitingToStartComponent>()
            .ForEach((Entity entity, in GameStateComponent gameState) => {
                if (gameState.GameState == GameState.WaitingToStart) {
                    ecb.SetComponentEnabled<WaitingToStartComponent>(entity, true);
                }
            }).Schedule();
        
        Entities
            .WithAll<CanCountdownToStartComponent, IsStateChangedComponent>()
            .WithNone<CountdownToStartComponent>()
            .ForEach((Entity entity, in GameStateComponent gameState) => {
                if (gameState.GameState == GameState.CountdownToStart) {
                    ecb.SetComponentEnabled<CountdownToStartComponent>(entity, true);
                }
            }).Schedule();
        
        Entities
            .WithAll<CanHavePlayingTimerComponent, IsStateChangedComponent>()
            .WithNone<PlayingTimerComponent>()
            .ForEach((Entity entity, in GameStateComponent gameState) => {
                if (gameState.GameState == GameState.GamePlaying) {
                    ecb.SetComponentEnabled<PlayingTimerComponent>(entity, true);
                }
            }).Schedule();
        
        Entities
            .WithAll<CanHaveGameOverStateComponent, IsStateChangedComponent>()
            .WithNone<IsGameOverStateComponent>()
            .ForEach((Entity entity, in GameStateComponent gameState) => {
                if (gameState.GameState == GameState.GameOver) {
                    ecb.SetComponentEnabled<IsGameOverStateComponent>(entity, true);
                }
            }).Schedule();

        Entities
            .WithAll<CanGenerateOrdersComponent>()
            .ForEach((Entity entity) => {
                if (gameStateNative[0].IsGameActive()) {
                    ecb.SetComponentEnabled<IsGenerateOrdersRestrictedComponent>(entity, false);
                }
                else {
                    ecb.SetComponentEnabled<IsGenerateOrdersRestrictedComponent>(entity, true);
                }
            }).Schedule();
        
        Entities
            .WithDisposeOnCompletion(gameStateNative)
            .WithAll<CanBeSelectedComponent>()
            .ForEach((Entity entity) => {
                if (gameStateNative[0].IsGameActive()) {
                    ecb.SetComponentEnabled<IsSelectionRestricted>(entity, false);
                }
                else {
                    ecb.SetComponentEnabled<IsSelectionRestricted>(entity, true);
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, false);
                }
            }).Schedule();

        Entities
            .WithAll<IsStateChangedComponent>()
            .ForEach((Entity entity) => {
                ecb.SetComponentEnabled<IsStateChangedComponent>(entity, false);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class StateChangeMonitoringSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .ForEach((Entity entity, ref GameStateComponent gameState) => {
                if (gameState.GameState == gameState.PreviousState) {
                    return;
                }

                gameState.PreviousState = gameState.GameState;
                ecb.SetComponentEnabled<IsStateChangedComponent>(entity, true);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}

partial class WaitingToStartSystem : SystemBase {
    protected override void OnUpdate() {
        float dt = SystemAPI.Time.DeltaTime;
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .ForEach((Entity entity, ref GameStateComponent gameState, ref WaitingToStartComponent waitingToStart) => {
                waitingToStart.Timer += dt;
                if (waitingToStart.Timer < waitingToStart.Goal) {
                    return;
                }

                waitingToStart.Timer = 0f;
                gameState.GameState = GameState.CountdownToStart;
                ecb.SetComponentEnabled<WaitingToStartComponent>(entity, false);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}
