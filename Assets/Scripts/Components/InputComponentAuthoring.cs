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
        }
    }
}

public struct InputComponent : IComponentData {
    public float2 MoveInput;
}