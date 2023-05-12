using System;
using Unity.Entities;
using Unity.Transforms;

partial class SelectedItemInteractSystem : SystemBase {
    protected override void OnCreate() {
        PlayerInputBuffer.Instance.OnInteractAction += InstanceOnOnInteractAction;
    }

    private void InstanceOnOnInteractAction(object sender, EventArgs e) {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        
        Entities
            .WithAll<IsSelectedItemComponent>()
            .ForEach((Entity entity, in SpawnPrefabComponent ingredientPrefab, in InteractedPlayerIngredientComponent playerIngredient) => {
                if (playerIngredient.Ingredient.Entity != Entity.Null) {
                    return;
                }

                Entity spawnedEntity = ecb.Instantiate(ingredientPrefab.Prefab);
                ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(spawnedEntity, true);
            })
            .Schedule();
        
        Entities
            .WithAll<IsSelectedItemComponent, CanHaveIsOpenAnimationComponent>()
            .WithNone<IsOpenAnimationComponent>()
            .ForEach((Entity entity) => {
                ecb.SetComponentEnabled<IsOpenAnimationComponent>(entity, true);
            }).Schedule();
        
        Entities
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent>()
            .ForEach((Entity entity, ref IngredientEntityComponent ingredient, 
                ref InteractedPlayerIngredientComponent playerIngredient, in ItemPlaceholderComponent itemPlaceholder) => {
                // If player holds something - put it on counter.
                if (playerIngredient.Ingredient.Entity != Entity.Null && ingredient.Entity == Entity.Null) {
                    ecb.SetComponent(playerIngredient.Ingredient.Entity, new Parent {
                        Value = itemPlaceholder.Entity
                    });
                    ecb.SetComponent(playerIngredient.Ingredient.Entity, itemPlaceholder.LocalPosition);
                    ecb.SetComponentEnabled<IngredientMustBeReleaseComponent>(playerIngredient.Ingredient.Entity, true);
                    ingredient = playerIngredient.Ingredient;
                    playerIngredient = new InteractedPlayerIngredientComponent();
                // If player holds nothing and there is something on the counter - take it.
                } else if (playerIngredient.Ingredient.Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    ecb.SetComponentEnabled<IngredientMustBeGrabbedComponent>(ingredient.Entity, true);
                    playerIngredient.Ingredient = ingredient;
                    ingredient = new IngredientEntityComponent();
                }
            })
            .Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    protected override void OnUpdate() {
        
    }
}