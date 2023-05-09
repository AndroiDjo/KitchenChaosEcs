using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class InputComponentAuthoring : MonoBehaviour {
    class Baker : Baker<InputComponentAuthoring> {
        public override void Bake(InputComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new InputComponent {
                MoveInput = float2.zero
            });
            AddComponent<LastInputDirectionComponent>(entity);
        }
    }
}

public struct InputComponent : IComponentData, IEnableableComponent {
    public float2 MoveInput;
}

public struct LastInputDirectionComponent : IComponentData {
    public float2 Value;
}