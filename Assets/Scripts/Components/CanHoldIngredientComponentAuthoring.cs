using UnityEngine;
using Unity.Entities;

public class CanHoldIngredientComponentAuthoring : MonoBehaviour {
    public HolderType HolderType;
    
    class Baker : Baker<CanHoldIngredientComponentAuthoring> {
        public override void Bake(CanHoldIngredientComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CanHoldIngredientComponent {
                HolderType = authoring.HolderType
            });
        }
    }
}

public struct CanHoldIngredientComponent : IComponentData {
    public HolderType HolderType;
}

public enum HolderType {
    None,
    Player,
    ClearCounter,
    CuttingCounter,
    StoveCounter
}