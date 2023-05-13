using UnityEngine;
using Unity.Entities;

public class IngredientNextStagePrefabComponentAuthoring : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    class Baker : Baker<IngredientNextStagePrefabComponentAuthoring> {
        public override void Bake(IngredientNextStagePrefabComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new IngredientNextStagePrefabComponent {
                Prefab = GetEntity(authoring.prefab, TransformUsageFlags.None)
            });
        }
    }
}

public struct IngredientNextStagePrefabComponent : IComponentData {
    public Entity Prefab;
}