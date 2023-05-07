using UnityEngine;
using Unity.Entities;

public class RotateSpeedComponentAuthoring : MonoBehaviour {
    [SerializeField] private float RotateSpeed;

    class Baker : Baker<RotateSpeedComponentAuthoring> {
        public override void Bake(RotateSpeedComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotateSpeedComponent { Value = authoring.RotateSpeed });
        }
    }
}

public struct RotateSpeedComponent : IComponentData {
    public float Value;
}