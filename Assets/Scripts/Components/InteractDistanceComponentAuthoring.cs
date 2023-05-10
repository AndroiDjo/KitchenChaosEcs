using Unity.Entities;
using UnityEngine;

public class InteractDistanceComponentAuthoring : MonoBehaviour {
    [SerializeField] private float interactDistance = 1f;

    class Baker : Baker<InteractDistanceComponentAuthoring> {
        public override void Bake(InteractDistanceComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new InteractDistanceComponent { Value = authoring.interactDistance });
        }
    }
}

public struct InteractDistanceComponent : IComponentData {
    public float Value;
}