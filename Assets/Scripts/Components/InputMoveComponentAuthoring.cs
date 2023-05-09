using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class InputMoveComponentAuthoring : MonoBehaviour {
    class Baker : Baker<InputMoveComponentAuthoring> {
        public override void Bake(InputMoveComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new InputMoveComponent {
                Value = float2.zero
            });
            AddComponent<LastInputDirectionComponent>(entity);
        }
    }
}

public struct InputMoveComponent : IComponentData {
    public float2 Value;
}

public struct LastInputDirectionComponent : IComponentData {
    public float2 Value;
}