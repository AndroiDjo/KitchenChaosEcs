using UnityEngine;
using Unity.Entities;

public class InputInteractComponentAuthoring : MonoBehaviour {
    class Baker : Baker<InputInteractComponentAuthoring> {
        public override void Bake(InputInteractComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<InputInteractComponent>(entity);
            SetComponentEnabled<InputInteractComponent>(entity, false);
            AddComponent<CanHaveInputInteractComponent>(entity);
        }
    }
}

public struct InputInteractComponent : IComponentData, IEnableableComponent {}

public struct CanHaveInputInteractComponent : IComponentData {}