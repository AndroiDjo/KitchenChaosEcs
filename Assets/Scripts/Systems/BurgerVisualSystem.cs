using Unity.Entities;
using Unity.Transforms;

partial class BurgerVisualSystem : SystemBase {
    protected override void OnUpdate() {
        var ecbSystem = this.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
        var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();
        var enabledLookup = SystemAPI.GetComponentLookup<Disabled>(true);
        
        Entities
            .WithReadOnly(enabledLookup)
            .ForEach((
                int entityInQueryIndex,
                // Just for child visual position refresh after enabling ... facepalm.
                ref LocalTransform localTransform,
                in DynamicBuffer<CompleteBurgerVisualBufferComponent> completeVisualBuffer,
                in DynamicBuffer<BurgerIngredientsBufferComponent> burgerIngredientsBuffer
                ) => {
                foreach (var burgerIngredient in burgerIngredientsBuffer) {
                    foreach (var ingredientVisual in completeVisualBuffer) {
                        if (burgerIngredient.BurgerIngredient.IngredientType == ingredientVisual.IngredientType &&
                            enabledLookup.HasComponent(ingredientVisual.Visual)) {
                            ecb.SetEnabled(entityInQueryIndex, ingredientVisual.Visual, true);
                        }
                    }
                }
            }).ScheduleParallel();
        
        ecbSystem.AddJobHandleForProducer(this.Dependency);
    }
}