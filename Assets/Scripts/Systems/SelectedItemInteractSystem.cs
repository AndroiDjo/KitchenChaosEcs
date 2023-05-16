using System;
using Helpers;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(SetSelectedItemSystem))]
partial class SelectedItemInteractSystem : SystemBase {

    private static bool TryPutOnPlate(ref EntityCommandBuffer ecb, in IngredientEntityComponent playerIngredient,
        in IngredientEntityComponent counterIngredient) {
        
        if (playerIngredient.Entity == Entity.Null || counterIngredient.Entity == Entity.Null) {
            return false;
        }

        if (playerIngredient.IngredientType.IngredientType == IngredientType.Plate &&
            counterIngredient.IngredientType.IsBurgerIngredient()) {
            ecb.AppendToBuffer(playerIngredient.Entity, new BurgerIngredientsBufferComponent {
                BurgerIngredient = counterIngredient.IngredientType
            });
            ecb.DestroyEntity(counterIngredient.Entity);
            return true;
        } else if (playerIngredient.IngredientType.IsBurgerIngredient()  &&
                   counterIngredient.IngredientType.IngredientType == IngredientType.Plate) {
            ecb.AppendToBuffer(counterIngredient.Entity, new BurgerIngredientsBufferComponent {
                BurgerIngredient = playerIngredient.IngredientType
            });
            ecb.DestroyEntity(playerIngredient.Entity);
            return true;
        }

        return false;
    }
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
        
        // Interact with clear counter.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent>()
            .WithNone<CanCutIngredientComponent, CanFryIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null) {
                    // If player holds something - put it on counter.
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    // If player holds nothing and there is something on the counter - take it.
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                } else {
                    TryPutOnPlate(ref ecb, playerIngredientNative[0], ingredient);
                }
            }).Schedule();
        
        // Interact with cut counter.
        var cutCounterLookup = SystemAPI.GetComponentLookup<CutCounterComponent>(true);
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithReadOnly(cutCounterLookup)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent, CanCutIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null &&
                    cutCounterLookup.HasComponent(playerIngredientNative[0].Entity)) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                } else {
                    TryPutOnPlate(ref ecb, playerIngredientNative[0], ingredient);
                }
            }).Schedule();
        
        // Interact with frying counter.
        var fryCounterLookup = SystemAPI.GetComponentLookup<FryCounterComponent>(true);
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithReadOnly(fryCounterLookup)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent, CanFryIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null &&
                    fryCounterLookup.HasComponent(playerIngredientNative[0].Entity)) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                } else {
                    TryPutOnPlate(ref ecb, playerIngredientNative[0], ingredient);
                }
            }).Schedule();
        
        // Interact with plates counter.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithAll<IsSelectedItemComponent>()
            .ForEach((ref DynamicBuffer<ItemsBufferComponent> itemsBuffer) => {
                if (playerIngredientNative[0].Entity != Entity.Null || itemsBuffer.Length == 0) {
                    return;
                }

                int lastIndex = itemsBuffer.Length - 1;
                Entity topPlate = itemsBuffer[lastIndex].Item;
                EntitySystemHelper.SetNewParentToEntity(ref ecb, topPlate, playerItemPlaceholderNative[0], false);
                itemsBuffer.RemoveAt(lastIndex);
            }).Schedule();
        
        // Container counter spawn ingredient.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithDisposeOnCompletion(playerItemPlaceholderNative)
            .WithAll<IsSelectedItemComponent, SpawnOnInteractComponent>()
            .ForEach((in SpawnPrefabComponent ingredientPrefab) => {
                if (playerIngredientNative[0].Entity != Entity.Null) {
                    return;
                }
        
                Entity spawnedEntity = ecb.Instantiate(ingredientPrefab.Prefab);
                EntitySystemHelper.SetNewParentToEntity(ref ecb, spawnedEntity, playerItemPlaceholderNative[0], true);
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