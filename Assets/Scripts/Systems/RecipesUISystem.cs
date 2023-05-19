using UnityEngine;
using Unity.Entities;

public partial struct RecipesUISystem : ISystem, ISystemStartStop {

    private RecipesListComponent _recipesList;

    public void OnCreate(ref SystemState state) {
        state.RequireForUpdate<RecipesListComponent>();
    }

    public void OnStartRunning(ref SystemState state) {
        _recipesList = SystemAPI.GetSingleton<RecipesListComponent>();
    }

    public void OnStopRunning(ref SystemState state) {
        
    }

    public void OnUpdate(ref SystemState state) {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        
        foreach (var (generateUIComponent, entity) in 
                 SystemAPI.Query<NeedGenerateUIForRecipeComponent>()
                     .WithEntityAccess()) {
            
            ref var recipe = ref _recipesList.RecipesReference.Value.Recipes[generateUIComponent.RecipeIndex];
            GameObject spawnedElement = DeliveryManagerUI.Instance.CreateElement(
                recipe.RecipeName.ToString(),
                recipe.Ingredients.ToArray()
                );
            
            ecb.AddComponent(entity, new GameObjectBindingComponent{ GameObject = spawnedElement });
            SystemAPI.SetComponentEnabled<NeedGenerateUIForRecipeComponent>(entity, false);
        }
    }
}