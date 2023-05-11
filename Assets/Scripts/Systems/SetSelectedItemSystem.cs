using Unity.Collections;
using Unity.Entities;

partial class SetSelectedItemSystem : SystemBase {
    protected override void OnUpdate() {
        NativeArray<Entity> selectedItemNative = new NativeArray<Entity>(1, Allocator.TempJob);
        NativeArray<ItemPlaceholderComponent> itemPlaceholderNative =
            new NativeArray<ItemPlaceholderComponent>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((in PlayerInteractTargetComponent interactTarget, in ItemPlaceholderComponent itemPlaceholder) => {
                selectedItemNative[0] = interactTarget.TargetEntity;
                itemPlaceholderNative[0] = itemPlaceholder;
            })
            .Schedule();

        Entities
            .WithAll<CanBeSelectedComponent, IsSelectedItemComponent>()
            .ForEach((Entity entity, in SelectedItemVisualComponent selectedVisual) => {
                if (!entity.Equals(selectedItemNative[0])) {
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, false);
                    ecb.SetEnabled(selectedVisual.Entity, false);
                }
            })
            .Schedule();
        
        Entities
            .WithAll<CanBeSelectedComponent>()
            .WithNone<IsSelectedItemComponent>()
            .WithDisposeOnCompletion(selectedItemNative)
            .WithDisposeOnCompletion(itemPlaceholderNative)
            .ForEach((Entity entity, ref InteractedPlayerItemPlaceholderComponent playerItemPlaceholder, in SelectedItemVisualComponent selectedVisual) => {
                if (entity.Equals(selectedItemNative[0])) {
                    playerItemPlaceholder.Placeholder = itemPlaceholderNative[0];
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, true);
                    ecb.SetEnabled(selectedVisual.Entity, true);
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}