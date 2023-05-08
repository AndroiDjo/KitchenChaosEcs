using Unity.Burst;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
partial struct GameObjectFollowEntitySystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        foreach (var (goTransform, entityTransform) in SystemAPI.Query<GameObjectTransformComponent, LocalTransform>()) {
            goTransform.Transform.position = entityTransform.Position;
            goTransform.Transform.rotation = entityTransform.Rotation;
            goTransform.Transform.localScale =
                new Vector3(entityTransform.Scale, entityTransform.Scale, entityTransform.Scale);
        }
    }
}