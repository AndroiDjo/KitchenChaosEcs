using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SelectedItemInteractSystem))]
partial class GrabbingSystem : SystemBase {
    protected override void OnUpdate() {
        var toGrabEntityNative = new NativeArray<Entity>(1, Allocator.TempJob);
        var lastInteractNative = new NativeArray<LastInteractedEntityComponent>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        // Find ingredient, that must be grabbed.
        Entities
            .WithAll<IngredientMustBeGrabbedComponent>()
            .ForEach((Entity entity, in LastInteractedEntityComponent lastInteract) => {
                toGrabEntityNative[0] = entity;
                lastInteractNative[0] = lastInteract;
                ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(entity, false);
            }).Schedule();
        
        // Process grabbing.
        Entities
            .WithAll<MustGrabIngredientComponent>()
            .WithDisposeOnCompletion(toGrabEntityNative)
            .ForEach((Entity entity, ref IngredientEntityComponent ingredientEntity, in ItemPlaceholderComponent itemPlaceholder) => {
                ecb.SetComponentEnabled<MustGrabIngredientComponent>(entity, false);
                if (toGrabEntityNative[0] == Entity.Null)
                    return;
                
                // Destroy old entity if it still exists.
                if (ingredientEntity.Entity != Entity.Null) {
                    ecb.DestroyEntity(ingredientEntity.Entity);
                }

                // Old entity, that used to hold ingredient, must forgot about it.
                if (lastInteractNative[0].Entity != Entity.Null) {
                    ecb.SetComponent(lastInteractNative[0].Entity, new IngredientEntityComponent());
                }
                
                // Set new parent to ingredient.
                ingredientEntity.Entity = toGrabEntityNative[0];
                ecb.SetComponent(ingredientEntity.Entity, new LastInteractedEntityComponent{Entity = entity});
                ecb.AddComponent(ingredientEntity.Entity, new Parent { Value = itemPlaceholder.Entity});
                ecb.SetComponent(ingredientEntity.Entity, itemPlaceholder.LocalPosition);
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}