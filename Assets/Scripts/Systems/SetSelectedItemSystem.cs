using Unity.Collections;
using Unity.Entities;

partial class SetSelectedItemSystem : SystemBase {
    protected override void OnUpdate() {
        NativeArray<Entity> selectedItemNative = new NativeArray<Entity>(1, Allocator.TempJob);
        var lastInteractedNative = new NativeArray<LastInteractedEntityComponent>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((Entity entity, in PlayerInteractTargetComponent interactTarget, in IngredientEntityComponent ingredientEntity) => {
                selectedItemNative[0] = interactTarget.TargetEntity;
                lastInteractedNative[0] = new LastInteractedEntityComponent {
                    Entity = entity,
                    Ingredient = ingredientEntity
                };
            })
            .Schedule();

        Entities
            .WithAll<CanBeSelectedComponent, IsSelectedItemComponent>()
            .ForEach((Entity entity, ref LastInteractedEntityComponent lastInteracted, in SelectedItemVisualComponent selectedVisual) => {
                if (entity == selectedItemNative[0]) {
                    lastInteracted = lastInteractedNative[0];
                }
                else {
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, false);
                    ecb.SetEnabled(selectedVisual.Entity, false);
                }
            })
            .Schedule();
        
        Entities
            .WithAll<CanBeSelectedComponent>()
            .WithNone<IsSelectedItemComponent>()
            .WithDisposeOnCompletion(selectedItemNative)
            .WithDisposeOnCompletion(lastInteractedNative)
            .ForEach((Entity entity, ref LastInteractedEntityComponent lastInteracted, in SelectedItemVisualComponent selectedVisual) => {
                if (entity != selectedItemNative[0])
                    return;

                lastInteracted = lastInteractedNative[0];
                ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, true);
                ecb.SetEnabled(selectedVisual.Entity, true);
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}