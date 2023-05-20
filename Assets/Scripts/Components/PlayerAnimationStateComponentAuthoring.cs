using UnityEngine;
using Unity.Entities;

public class PlayerAnimationStateComponentAuthoring : MonoBehaviour {
    class Baker : Baker<PlayerAnimationStateComponentAuthoring> {
        public override void Bake(PlayerAnimationStateComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CanHaveWalkingAnimationComponent>(entity);
            AddComponent<CanPlayWalkingSoundComponent>(entity);
            AddComponent<PlayerIsWalkingComponent>(entity);
            SetComponentEnabled<PlayerIsWalkingComponent>(entity, false);
            AddComponent<PlayerIsWalkingSoundComponent>(entity);
            SetComponentEnabled<PlayerIsWalkingSoundComponent>(entity, false);
        }
    }
}

public struct PlayerIsWalkingComponent : IComponentData, IEnableableComponent {}
public struct CanHaveWalkingAnimationComponent : IComponentData {}
public struct PlayerIsWalkingSoundComponent : IComponentData, IEnableableComponent {}
public struct CanPlayWalkingSoundComponent : IComponentData {}

