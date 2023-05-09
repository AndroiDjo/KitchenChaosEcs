using Unity.Collections;
using Unity.Entities;

partial class SetSelectedItemSystem : SystemBase {
    protected override void OnUpdate() {
        NativeArray<Entity> selectedItemNative = new NativeArray<Entity>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        BufferLookup<LinkedEntityGroup> linkedGroupLookup = GetBufferLookup<LinkedEntityGroup>(true);

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((in PlayerInteractTargetComponent interactTarget) => {
                selectedItemNative[0] = interactTarget.TargetEntity;
            })
            .Schedule();

        Entities
            .WithReadOnly(linkedGroupLookup)
            .WithAll<CanBeSelectedComponent, IsSelectedItemComponent>()
            .ForEach((Entity entity, in SelectedItemVisualComponent selectedVisual) => {
                if (!entity.Equals(selectedItemNative[0])) {
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, false);
                    SetSelectedVisual(false, ecb, selectedVisual, linkedGroupLookup);
                }
            })
            .Schedule();
        
        Entities
            .WithReadOnly(linkedGroupLookup)
            .WithAll<CanBeSelectedComponent>()
            .WithNone<IsSelectedItemComponent>()
            .WithDisposeOnCompletion(selectedItemNative)
            .ForEach((Entity entity, in SelectedItemVisualComponent selectedVisual) => {
                if (entity.Equals(selectedItemNative[0])) {
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, true);
                    SetSelectedVisual(true, ecb, selectedVisual, linkedGroupLookup);
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    private static void SetSelectedVisual(bool enable, EntityCommandBuffer ecb, in SelectedItemVisualComponent selectedItemVisual, 
        BufferLookup<LinkedEntityGroup> linkedGroupLookup) {
        if (linkedGroupLookup.TryGetBuffer(selectedItemVisual.Entity, out DynamicBuffer<LinkedEntityGroup> linkedEntities)) {
            foreach (var linkedEntity in linkedEntities) {
                if (enable) {
                    ecb.RemoveComponent<Disabled>(linkedEntity.Value);
                }
                else {
                    ecb.AddComponent<Disabled>(linkedEntity.Value);
                }
            }
        }
    }
}