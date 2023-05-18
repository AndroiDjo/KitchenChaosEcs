using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

partial class RecipesGeneratorSystem : SystemBase {
    protected override void OnUpdate() {
        NativeArray<Random> Randoms = RandomsUtilitySystem.Randoms;
        float dt = SystemAPI.Time.DeltaTime;
        
        Entities
            .WithNativeDisableParallelForRestriction(Randoms)
            .ForEach((
                int nativeThreadIndex,
                ref DynamicBuffer<RecipesQueueElementComponent> recipesQueue,
                ref CanGenerateOrdersComponent canGenerateOrders,
                in RecipesListComponent recipesList
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

                recipesQueue.Add(new RecipesQueueElementComponent {
                    RecipeIndex = random.NextInt(recipesList.RecipesReference.Value.Recipes.Length)
                });
                
                Randoms[nativeThreadIndex] = random;
            }).Schedule();
    }
}