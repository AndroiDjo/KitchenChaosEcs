using Unity.Entities;
using UnityEngine;

public class RecipiesQueueBufferAuthoring : MonoBehaviour {
    class Baker : Baker<RecipiesQueueBufferAuthoring> {
        public override void Bake(RecipiesQueueBufferAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddBuffer<RecipesQueueElementComponent>(entity);
        }
    }
}

// RecipesQueueElementComponent stores index of Recipe in blob array.
public struct RecipesQueueElementComponent : IBufferElementData {
    public int RecipeIndex;
    public Entity EntityUI;
}