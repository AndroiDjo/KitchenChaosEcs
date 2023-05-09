using UnityEngine;
using Unity.Entities;

public class SelectedItemVisualComponentAuthoring : MonoBehaviour {
    
    [SerializeField]
    private GameObject selectedEntity;
    
    class Baker : Baker<SelectedItemVisualComponentAuthoring> {
        public override void Bake(SelectedItemVisualComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SelectedItemVisualComponent {
                Entity = GetEntity(authoring.selectedEntity, TransformUsageFlags.None)
            });
        }
    }
}

public struct SelectedItemVisualComponent : IComponentData {
    public Entity Entity;
}