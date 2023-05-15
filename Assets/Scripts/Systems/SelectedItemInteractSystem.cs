using System;
using Helpers;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SetSelectedItemSystem))]
partial class SelectedItemInteractSystem : SystemBase {
    protected override void OnCreate() {
        var playerInputActionsSystem = this.World.GetExistingSystemManaged<CustomInputSystem>();
        playerInputActionsSystem.OnInteractAction += InstanceOnOnInteractAction;
        playerInputActionsSystem.OnInteractAlternateAction += InstanceOnOnInteractAlternateAction;
    }

    private void InstanceOnOnInteractAction(object sender, EventArgs e) {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        var playerIngredientNative = new NativeArray<IngredientEntityComponent>(1, Allocator.TempJob);
        var playerItemPlaceholderNative = new NativeArray<ItemPlaceholderComponent>(1, Allocator.TempJob);

        Entities
            .WithAll<PlayerTagComponent>()
            .ForEach((Entity entity, in IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                playerIngredientNative[0] = ingredient;
                playerItemPlaceholderNative[0] = itemPlaceholder;
            }).Schedule();
        
        // Put ingredient to regular counter.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent>()
            .WithNone<CanCutIngredientComponent, CanFryIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null) {
                    // If player holds something - put it on counter.
                    EntitySystemHelper.SetNewParentToIngredient(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    // If player holds nothing and there is something on the counter - take it.
                    EntitySystemHelper.SetNewParentToIngredient(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                }
            }).Schedule();
        
        // Put ingredient to cut counter.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent, CanCutIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null &&
                    SystemAPI.HasComponent<CutCounterComponent>(playerIngredientNative[0].Entity)) {
                    EntitySystemHelper.SetNewParentToIngredient(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    EntitySystemHelper.SetNewParentToIngredient(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                }
            }).Schedule();
        
        // Put ingredient to frying counter.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent, CanFryIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null &&
                    SystemAPI.HasComponent<FryCounterComponent>(playerIngredientNative[0].Entity)) {
                    EntitySystemHelper.SetNewParentToIngredient(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    EntitySystemHelper.SetNewParentToIngredient(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                }
            }).Schedule();
        
        // Container counter spawn ingredient.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithDisposeOnCompletion(playerItemPlaceholderNative)
            .WithAll<IsSelectedItemComponent>()
            .ForEach((in SpawnPrefabComponent ingredientPrefab) => {
                if (playerIngredientNative[0].Entity != Entity.Null) {
                    return;
                }
        
                Entity spawnedEntity = ecb.Instantiate(ingredientPrefab.Prefab);
                EntitySystemHelper.SetNewParentToIngredient(ref ecb, spawnedEntity, playerItemPlaceholderNative[0], true);
            })
            .Schedule();
        
        // Do open animation for suitable counters.
        Entities
            .WithAll<IsSelectedItemComponent, CanHaveIsOpenAnimationComponent>()
            .WithNone<IsOpenAnimationComponent>()
            .ForEach((Entity entity) => {
                ecb.SetComponentEnabled<IsOpenAnimationComponent>(entity, true);
            }).Schedule();
        
        // Proceed trash counter.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithDisposeOnCompletion(playerIngredientNative)
            .WithAll<IsSelectedItemComponent, CanDestroyIngredientComponent>()
            .ForEach((Entity entity) => {
                if (playerIngredientNative[0].Entity != Entity.Null) {
                   ecb.DestroyEntity(playerIngredientNative[0].Entity);
                }
            }).Schedule();
        
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
                    ecb.SetComponentEnabled<TryToCutIngredientComponent>(ingredient.Entity, true);
                }
            }).Schedule();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }

    protected override void OnUpdate() {
        
    }
}