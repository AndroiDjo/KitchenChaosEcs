using UnityEngine;
using Unity.Entities;

public class IsCuttingAnimationComponentAuthoring : MonoBehaviour {
    class Baker : Baker<IsCuttingAnimationComponentAuthoring> {
        public override void Bake(IsCuttingAnimationComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanHaveIsCuttingAnimationComponent>(entity);
            AddComponent<IsCuttingAnimationComponent>(entity);
            SetComponentEnabled<IsCuttingAnimationComponent>(entity, false);
        }
    }
}
public struct IsCuttingAnimationComponent : IComponentData, IEnableableComponent {}

public struct CanHaveIsCuttingAnimationComponent : IComponentData {}