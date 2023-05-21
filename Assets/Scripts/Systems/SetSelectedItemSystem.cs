using Unity.Collections;
using Unity.Entities;

partial class SetSelectedItemSystem : SystemBase {
    protected override void OnUpdate() {
        NativeArray<Entity> selectedItemNative = new NativeArray<Entity>(1, Allocator.TempJob);
        NativeArray<GameStateComponent> gameStateNative = new NativeArray<GameStateComponent>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .ForEach((in GameStateComponent gameState) => {
                gameStateNative[0] = gameState;
            }).Schedule();
        
        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((Entity entity, in PlayerInteractTargetComponent interactTarget) => {
                selectedItemNative[0] = interactTarget.TargetEntity;
            })
            .Schedule();

        Entities
            .WithAll<CanBeSelectedComponent, IsSelectedItemComponent>()
            .ForEach((Entity entity, in SelectedItemVisualComponent selectedVisual) => {
                if (entity != selectedItemNative[0]) {
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, false);
                    ecb.SetEnabled(selectedVisual.Entity, false);
                }
            })
            .Schedule();
        
        Entities
            .WithAll<CanBeSelectedComponent>()
            .WithNone<IsSelectedItemComponent>()
            .WithDisposeOnCompletion(selectedItemNative)
            .WithDisposeOnCompletion(gameStateNative)
            .ForEach((Entity entity, in SelectedItemVisualComponent selectedVisual) => {
                if (!gameStateNative[0].IsGameActive() || entity != selectedItemNative[0])
                    return;

                ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, true);
                ecb.SetEnabled(selectedVisual.Entity, true);
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}