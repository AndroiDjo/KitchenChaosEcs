using UnityEngine;
using Unity.Entities;

public class PlayerInteractTargetComponentAuthoring : MonoBehaviour {
    class Baker : Baker<PlayerInteractTargetComponentAuthoring> {
        public override void Bake(PlayerInteractTargetComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<PlayerInteractTargetComponent>(entity);
        }
    }
}

public struct PlayerInteractTargetComponent : IComponentData {
    public Entity TargetEntity;
}