using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[RequireMatchingQueriesForUpdate]
partial class RecipesGeneratorSystem : SystemBase {

    private RecipesListComponent _recipesList;
    private EntityArchetype _UIEntityArchetype;

    protected override void OnCreate() {
        _UIEntityArchetype = EntityManager.CreateArchetype(
            typeof(MustBeCleanedBeforeDestroyComponent),
            typeof(NeedGenerateUIForRecipeComponent)
            );
    }

    protected override void OnStartRunning() {
        _recipesList = SystemAPI.GetSingleton<RecipesListComponent>();
    }

    protected override void OnUpdate() {
        NativeArray<Unity.Mathematics.Random> Randoms = RandomsUtilitySystem.Randoms;
        float dt = SystemAPI.Time.DeltaTime;
        RecipesListComponent recipesList = _recipesList;
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer();
        EntityArchetype UIArchetype = _UIEntityArchetype;
        
        Entities
            .WithNativeDisableParallelForRestriction(Randoms)
            .WithNone<IsGenerateOrdersRestrictedComponent>()
            .ForEach((
                int nativeThreadIndex,
                Entity entity,
                ref CanGenerateOrdersComponent canGenerateOrders,
                in DynamicBuffer<RecipesQueueElementComponent> recipesQueue
            ) => {
                if (recipesQueue.Length >= canGenerateOrders.OrdersLimit) {
                    return;
                }

                canGenerateOrders.Timer += dt;
                if (canGenerateOrders.Timer < canGenerateOrders.GenerateDelay) {
                    return;
                }

                canGenerateOrders.Timer = 0f;
                var random = Randoms[nativeThreadIndex];
                int recipeIndex = random.NextInt(recipesList.RecipesReference.Value.Recipes.Length);
                
                Entity entityUI = ecb.CreateEntity(UIArchetype);
                ecb.AppendToBuffer(entity, new RecipesQueueElementComponent {
                    RecipeIndex = recipeIndex,
                    EntityUI = entityUI
                });
                ecb.SetComponent(entityUI, new NeedGenerateUIForRecipeComponent{RecipeIndex = recipeIndex});
                
                Randoms[nativeThreadIndex] = random;
            }).Schedule();

        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}