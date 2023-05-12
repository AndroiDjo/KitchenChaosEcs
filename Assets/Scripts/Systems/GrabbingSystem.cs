using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SelectedItemInteractSystem))]
partial class GrabbingSystem : SystemBase {
    protected override void OnUpdate() {
        var toGrabEntityNative = new NativeArray<Entity>(1, Allocator.TempJob);
        var toReleaseEntityNative = new NativeArray<Entity>(1, Allocator.TempJob);
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<IngredientMustBeGrabbedComponent>()
            .ForEach((Entity entity) => {
                toGrabEntityNative[0] = entity;
                ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(entity, false);
            }).Schedule();
        
        Entities
            .WithAll<IngredientMustBeReleaseComponent>()
            .ForEach((Entity entity) => {
                toReleaseEntityNative[0] = entity;
                ecb.SetComponentEnabled<IngredientMustBeReleaseComponent>(entity, false);
            }).Schedule();

        Entities
            .WithAll<PlayerTagComponent, CanHoldIngredientComponent>()
            .WithDisposeOnCompletion(toGrabEntityNative)
            .WithDisposeOnCompletion(toReleaseEntityNative)
            .ForEach((ref IngredientEntityComponent ingredientEntity, in ItemPlaceholderComponent itemPlaceholder) => {
                if (toGrabEntityNative[0] != Entity.Null) {
                    ingredientEntity.Entity = toGrabEntityNative[0];
                    ecb.AddComponent(ingredientEntity.Entity, new Parent {
                        Value = itemPlaceholder.Entity
                    });
                    ecb.SetComponent(ingredientEntity.Entity, itemPlaceholder.LocalPosition);
                }

                if (toReleaseEntityNative[0] != Entity.Null && toReleaseEntityNative[0] == ingredientEntity.Entity) {
                    ingredientEntity = new IngredientEntityComponent();
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}