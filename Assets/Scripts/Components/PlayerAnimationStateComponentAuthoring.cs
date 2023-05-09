using UnityEngine;
using Unity.Entities;

public class PlayerAnimationStateComponentAuthoring : MonoBehaviour {
    class Baker : Baker<PlayerAnimationStateComponentAuthoring> {
        public override void Bake(PlayerAnimationStateComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<PlayerAnimationIsWalkingComponent>(entity);
        }
    }
}

public struct PlayerAnimationIsWalkingComponent : IComponentData, IEnableableComponent {}