using System;
using Helpers;
using Unity.Collections;
using Unity.Entities;

[UpdateAfter(typeof(SetSelectedItemSystem))]
partial class SelectedItemInteractSystem : SystemBase {

    private static bool TryPutOnPlate(ref EntityCommandBuffer ecb, in IngredientEntityComponent playerIngredient,
        in IngredientEntityComponent counterIngredient, in BufferLookup<BurgerIngredientsBufferComponent> ingredientsBufferLookup) {
        
        if (playerIngredient.Entity == Entity.Null || counterIngredient.Entity == Entity.Null) {
            return false;
        }

        IngredientEntityComponent plateEntity = new IngredientEntityComponent();
        IngredientEntityComponent ingredientEntity = new IngredientEntityComponent();
        bool canPutIngredientOnPlate = false;
        if (playerIngredient.IngredientType.IngredientType == IngredientType.Plate &&
            counterIngredient.IngredientType.IsBurgerIngredient()) {
            plateEntity = playerIngredient;
            ingredientEntity = counterIngredient;
            canPutIngredientOnPlate = true;
        } else if (playerIngredient.IngredientType.IsBurgerIngredient()  &&
                   counterIngredient.IngredientType.IngredientType == IngredientType.Plate) {
            plateEntity = counterIngredient;
            ingredientEntity = playerIngredient;
            canPutIngredientOnPlate = true;
        }

        if (!canPutIngredientOnPlate) return false;

        if (!ingredientsBufferLookup.TryGetBuffer(plateEntity.Entity, out DynamicBuffer<BurgerIngredientsBufferComponent> ingredientsBuffer)) {
            return false;
        }

        if (ingredientsBuffer.Length >= GameObjectIngredientIconsUIComponent.INGREDIENT_ICONS_LIMIT) {
            return false;
        }
        
        ecb.AppendToBuffer(plateEntity.Entity, new BurgerIngredientsBufferComponent {
            BurgerIngredient = ingredientEntity.IngredientType
        });
        ecb.DestroyEntity(ingredientEntity.Entity);
        ecb.SetComponentEnabled<NeedUpdateUIComponent>(plateEntity.Entity, true);

        return true;
    }
    protected override void OnCreate() {
        var playerInputActionsSystem = this.World.GetExistingSystemManaged<CustomInputSystem>();
        playerInputActionsSystem.OnInteractAction += InstanceOnOnInteractAction;
        playerInputActionsSystem.OnInteractAlternateAction += InstanceOnOnInteractAlternateAction;
    }

    private void InstanceOnOnInteractAction(object sender, EventArgs e) {
        var ecbSystem = this.World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();
        var ingredientBufferLookup = SystemAPI.GetBufferLookup<BurgerIngredientsBufferComponent>(true);
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
            .WithReadOnly(ingredientBufferLookup)
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
                    TryPutOnPlate(ref ecb, playerIngredientNative[0], ingredient, ingredientBufferLookup);
                }
            }).Schedule();
        
        // Interact with cut counter.
        var cutCounterLookup = SystemAPI.GetComponentLookup<CutCounterComponent>(true);
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithReadOnly(cutCounterLookup)
            .WithReadOnly(ingredientBufferLookup)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent, CanCutIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null &&
                    cutCounterLookup.HasComponent(playerIngredientNative[0].Entity)) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                } else {
                    TryPutOnPlate(ref ecb, playerIngredientNative[0], ingredient, ingredientBufferLookup);
                }
            }).Schedule();
        
        // Interact with frying counter.
        var fryCounterLookup = SystemAPI.GetComponentLookup<FryCounterComponent>(true);
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(playerItemPlaceholderNative)
            .WithReadOnly(fryCounterLookup)
            .WithReadOnly(ingredientBufferLookup)
            .WithAll<IsSelectedItemComponent, CanHoldIngredientComponent, CanFryIngredientComponent>()
            .ForEach((ref IngredientEntityComponent ingredient, in ItemPlaceholderComponent itemPlaceholder) => {
                if (playerIngredientNative[0].Entity != Entity.Null && ingredient.Entity == Entity.Null &&
                    fryCounterLookup.HasComponent(playerIngredientNative[0].Entity)) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, playerIngredientNative[0].Entity, itemPlaceholder, false);
                } else if (playerIngredientNative[0].Entity == Entity.Null && ingredient.Entity != Entity.Null) {
                    EntitySystemHelper.SetNewParentToEntity(ref ecb, ingredient.Entity, playerItemPlaceholderNative[0], false);
                } else {
                    TryPutOnPlate(ref ecb, playerIngredientNative[0], ingredient, ingredientBufferLookup);
                }
            }).Schedule();
        
        // Interact with delivery counter.
        Entities
            .WithReadOnly(playerIngredientNative)
            .WithReadOnly(ingredientBufferLookup)
            .WithAll<IsSelectedItemComponent, CanDeliverMealsComponent>()
            .ForEach((ref DynamicBuffer<RecipesQueueElementComponent> recipesQueue, in RecipesListComponent recipesListBlob) => {

                if (playerIngredientNative[0].Entity == Entity.Null ||
                    playerIngredientNative[0].IngredientType.IngredientType != IngredientType.Plate) {
                    return;
                }

                if (!ingredientBufferLookup.TryGetBuffer(playerIngredientNative[0].Entity,
                    out DynamicBuffer<BurgerIngredientsBufferComponent> ingredientsOnThePlate)) {
                    return;
                }

                var ingredientsOnThePlateArray = ingredientsOnThePlate.ToNativeArray(Allocator.Temp);
                ingredientsOnThePlateArray.Sort();

                for (int recipeQueueIndex = 0; recipeQueueIndex < recipesQueue.Length; recipeQueueIndex++) {
                    ref Recipe recipe = ref recipesListBlob.RecipesReference.Value.Recipes[recipesQueue[recipeQueueIndex].RecipeIndex];
                    
                    if (recipe.Ingredients.Length != ingredientsOnThePlateArray.Length) {
                        continue;
                    }

                    bool ingredientsAreEqual = true;
                    for (int i = 0; i < ingredientsOnThePlateArray.Length; i++) {
                        if (ingredientsOnThePlateArray[i].BurgerIngredient.IngredientType != recipe.Ingredients[i]) {
                            ingredientsAreEqual = false;
                            break;
                        }
                    }

                    if (ingredientsAreEqual) {
                        recipesQueue.RemoveAt(recipeQueueIndex);
                        ecb.DestroyEntity(playerIngredientNative[0].Entity);
                        return;
                    }
                }
            })
            .Schedule();
        
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