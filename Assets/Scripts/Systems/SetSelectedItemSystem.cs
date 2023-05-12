using Unity.Collections;
using Unity.Entities;

partial class SetSelectedItemSystem : SystemBase {
    protected override void OnUpdate() {
        NativeArray<Entity> selectedItemNative = new NativeArray<Entity>(1, Allocator.TempJob);
        NativeArray<IngredientEntityComponent> ingredientEntityNative =
            new NativeArray<IngredientEntityComponent>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((in PlayerInteractTargetComponent interactTarget, in IngredientEntityComponent ingredientEntity) => {
                selectedItemNative[0] = interactTarget.TargetEntity;
                ingredientEntityNative[0] = ingredientEntity;
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
            .WithDisposeOnCompletion(ingredientEntityNative)
            .ForEach((Entity entity, ref InteractedPlayerIngredientComponent playerIngredient, in SelectedItemVisualComponent selectedVisual) => {
                if (entity == selectedItemNative[0]) {
                    playerIngredient.Ingredient = ingredientEntityNative[0];
                    ecb.SetComponentEnabled<IsSelectedItemComponent>(entity, true);
                    ecb.SetEnabled(selectedVisual.Entity, true);
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}