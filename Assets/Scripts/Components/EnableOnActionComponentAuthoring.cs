using UnityEngine;
using Unity.Entities;

public class EnableOnActionComponentAuthoring : MonoBehaviour {
    public GameObject Entity;

    class Baker : Baker<EnableOnActionComponentAuthoring> {
        public override void Bake(EnableOnActionComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnableOnActionComponent {
                Entity = GetEntity(authoring.Entity, TransformUsageFlags.None)
            });
        }
    }
}

public struct EnableOnActionComponent : IComponentData {
    public Entity Entity;
    public bool Enabled;
}