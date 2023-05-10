using Unity.Entities;
using UnityEngine;

public class CollisionToleranceComponentAuthoring : MonoBehaviour {
    [SerializeField] private float collisionTolerance = 3f;

    class Baker : Baker<CollisionToleranceComponentAuthoring> {
        public override void Bake(CollisionToleranceComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CollisionToleranceComponent { Value = authoring.collisionTolerance });
        }
    }
}

public struct CollisionToleranceComponent : IComponentData {
    public float Value;
}