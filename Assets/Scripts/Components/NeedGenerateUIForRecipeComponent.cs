using Unity.Entities;

public struct NeedGenerateUIForRecipeComponent : IComponentData, IEnableableComponent {
    public int RecipeIndex;
}