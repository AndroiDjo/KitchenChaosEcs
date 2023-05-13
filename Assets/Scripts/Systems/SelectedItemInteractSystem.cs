using System;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SetSelectedItemSystem))]
partial class SelectedItemInteractSystem : SystemBase {
    protected override void OnCreate() {
        PlayerInputBuffer.Instance.OnInteractAction += InstanceOnOnInteractAction;
        PlayerInputBuffer.Instance.OnInteractAlternateAction += InstanceOnOnInteractAlternateAction;
    }

    private void InstanceOnOnInteractAction(object sender, EventArgs e) {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        
        // Container counters spawn ingredient if it possible.
        Entities
            .WithAll<IsSelectedItemComponent>()
            .ForEach((Entity entity, LastInteractedEntityComponent lastInteracted, in SpawnPrefabComponent ingredientPrefab) => {
                if (lastInteracted.Ingredient.Entity != Entity.Null || lastInteracted.Entity == Entity.Null) {
                    return;
                }

                Entity spawnedEntity = ecb.Instantiate(ingredientPrefab.Prefab);
                ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(spawnedEntity, true);
                ecb.SetComponentEnabled<MustGrabIngredientComponent>(lastInteracted.Entity, true);
            })
            .Schedule();
        
        // Do open animation for suitable counters.
        Entities
            .WithAll<IsSelectedItemComponent, CanHaveIsOpenAnimationComponent>()
            .WithNone<IsOpenAnimationComponent>()
            .ForEach((Entity entity) => {
                ecb.SetComponentEnabled<IsOpenAnimationComponent>(entity, true);
            }).Schedule();
        
        // Initialize put/get ingredient process.
        Entities
            .WithAll<IsSelectedItemComponent, CanGrabIngredientComponent>()
            .ForEach((Entity entity,  
                ref LastInteractedEntityComponent lastInteracted,
                in IngredientEntityComponent ingredient) => {
                
                // If player holds something - put it on counter.
                if (lastInteracted.Ingredient.Entity != Entity.Null && ingredient.Entity == Entity.Null) {
                    ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(lastInteracted.Ingredient.Entity, true);
                    ecb.SetComponentEnabled<MustGrabIngredientComponent>(entity, true);
                    
                    // If player holds nothing and there is something on the counter - take it.
                } else if (lastInteracted.Ingredient.Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(ingredient.Entity, true);
                    ecb.SetComponentEnabled<MustGrabIngredientComponent>(lastInteracted.Entity, true);
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
    
    private void InstanceOnOnInteractAlternateAction(object sender, EventArgs e) {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithAll<IsSelectedItemComponent, CanHaveIsCuttingAnimationComponent>()
            .WithNone<IsCuttingAnimationComponent>()
            .ForEach((Entity entity) => {
                ecb.SetComponentEnabled<IsCuttingAnimationComponent>(entity, true);
            }).Schedule();

        Entities
            .WithAll<IsSelectedItemComponent, CanCutIngredientComponent>()
            .ForEach((Entity entity, in IngredientEntityComponent ingredient) => {
                if (ingredient.Entity != Entity.Null) {
                    ecb.SetComponentEnabled<IngredientMustBeCuttedComponent>(ingredient.Entity, true);
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    protected override void OnUpdate() {
        
    }
}