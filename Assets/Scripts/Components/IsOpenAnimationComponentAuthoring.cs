using UnityEngine;
using Unity.Entities;

public class IsOpenAnimationComponentAuthoring : MonoBehaviour {
    class Baker : Baker<IsOpenAnimationComponentAuthoring> {
        public override void Bake(IsOpenAnimationComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanHaveIsOpenAnimationComponent>(entity);
            AddComponent<IsOpenAnimationComponent>(entity);
            SetComponentEnabled<IsOpenAnimationComponent>(entity, false);
        }
    }
}
public struct IsOpenAnimationComponent : IComponentData, IEnableableComponent {}

public struct CanHaveIsOpenAnimationComponent : IComponentData {}