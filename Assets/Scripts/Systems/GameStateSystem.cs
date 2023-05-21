using Unity.Entities;

partial class GameStateSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        
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
